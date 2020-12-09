using HidSharp;
using HidSharp.Reports.Input;
using System;
using DualSenseDotNet.IO;
using DualSenseDotNet.PrimitiveTypes;

namespace DualSenseDotNet {
    /// <summary> The data stream over which our <see cref="Controller"/> and the physical Dualsense controller communicate with eachother. </summary>
    internal class ControllerConnection {
        private HidStream connection;
        public ConnectionType ConnectionType { get; private set; }

        /// <summary> for receiving data from the controller. </summary>
        private byte[] inputBuffer;
        /// <summary> for sending data to the controller. </summary>
        private byte[] outputBuffer;

        private HidDeviceInputReceiver inputNotifier;

        /// <remarks> 78 bytes on a Sony PS5 controller using bluetooth, 64 if connected by wire. </remarks>
        public readonly int MaxInputReportLength;
        /// <summary> 547 bytes on a Sony PS5 controller using bluetooth, 48 if connected by wire. </summary>
        public readonly int MaxOutputReportLength; // 

        public event Action InputReportReceived;

        #region Contruction & Destruction
        internal ControllerConnection(HidStream connection) {
            this.connection = connection;
            if (connection.CanRead is false)
                throw new System.Exception();

            HidDevice device = connection.Device;
            inputNotifier = new HidDeviceInputReceiver(device.GetReportDescriptor());

            MaxInputReportLength = device.GetMaxInputReportLength();
            MaxOutputReportLength = device.GetMaxOutputReportLength();
            Console.WriteLine(MaxOutputReportLength);
            if (MaxInputReportLength is 78) {
                // enables bluetooth connection
                connection.GetFeature(new BluetoothEnableRequest().Serialize(ref outputBuffer, ConnectionType.Bluetooth));
                ConnectionType = ConnectionType.Bluetooth;
            }
            else if (MaxInputReportLength is 64)
                ConnectionType = ConnectionType.Wired;
            else
                throw new Exception("Controller connection type cannot be determined as either bluetooth or wired!");

            //connection.Device.GetReportDescriptor().GetReport(ReportType.Input, 49).


            inputBuffer = new byte[MaxInputReportLength];
            outputBuffer = new byte[MaxOutputReportLength];

            inputNotifier.Received += RaiseInputReceivedEvent;
            inputNotifier.Start(connection);
        }

        ~ControllerConnection() {
            inputNotifier.Received -= RaiseInputReceivedEvent;
            connection.Dispose();
        }
        #endregion Contruction & Destruction
        private void RaiseInputReceivedEvent(object @object, EventArgs arguments) => InputReportReceived?.Invoke();

        /// <summary> Sends a properly formatted set of instructions out to the controller. </summary>
        internal void Write<T>(HIDReport<T> outputReport) {
            outputReport.Serialize(ref outputBuffer, ConnectionType);
            connection.Write(outputBuffer);
            Clear(outputBuffer);
        }

        /// <summary> Reads the most recent message from the controller. </summary>
        internal StateReport Read() {
            connection.Read(inputBuffer);

            var report = new StateReport();

            report.Deserialize(inputBuffer, ConnectionType);

            //Clear(inputBuffer);
            return report;
        }

        /// <summary> Erases an array of bytes. </summary>
        private void Clear(byte[] buffer) {
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = 0;
        }

        public void PrintStreamContents() {
            connection.Read(inputBuffer);
            Console.WriteLine($"Report Type: {inputBuffer[0]}");
            Console.WriteLine($"Flags: {Convert.ToString(inputBuffer[1], 2).PadLeft(8, '0')}-{Convert.ToString(inputBuffer[2], 2).PadLeft(8, '0').PadLeft(8, '0')}");
            Console.WriteLine($"Left Low Frequency Motor State: {inputBuffer[3]}");
            Console.WriteLine($"Right High Frequency Motor State: {inputBuffer[4]}");
            Console.WriteLine($"Headphone Out Volume: {inputBuffer[5]}");
            Console.WriteLine($"Internal Speaker Volume: {inputBuffer[6]}");
            Console.WriteLine($"Microphone Volume: {inputBuffer[7]}");
            Console.WriteLine($"Audio Flags: {Convert.ToString(inputBuffer[8], 2).PadLeft(8, '0').PadLeft(8, '0')}");
            Console.WriteLine($"Microphone LED Mode: {Convert.ToString(inputBuffer[9], 2).PadLeft(8, '0').PadLeft(8, '0')}");
            Console.WriteLine($"Mute Flags: {inputBuffer[10].ToString().PadLeft(8, '0')}");
            Console.WriteLine();
            Console.WriteLine($"Right Trigger Motor Mode: {Convert.ToString(inputBuffer[11], 2).PadLeft(8, '0').PadLeft(8, '0')}");
            Console.WriteLine($"Right Trigger Start Of Resistance: {inputBuffer[12]}");
            Console.WriteLine($"Right Trigger State: {inputBuffer[13]}");
            Console.WriteLine($"Right Trigger Force Exerted In Range (mode 2): {inputBuffer[14]}");
            Console.WriteLine($"Strength of Effect Near Release State (requires modes 4 & 20): {inputBuffer[15]}");
            Console.WriteLine($"Strength of Effect Near Middle State (requires modes 4 & 20): {inputBuffer[16]}");
            Console.WriteLine($"Strength of Effect At Pressed State (requires modes 4 & 20): {inputBuffer[17]}");
            Console.WriteLine($"Effect Actuation Frequency (Hz requires modes 4 & 20): {inputBuffer[20]}");
            Console.WriteLine();
            Console.WriteLine($"Left Trigger Motor Mode: {Convert.ToString(inputBuffer[22], 2).PadLeft(8, '0')}");
            Console.WriteLine($"Left Trigger Start Of Resistance: {inputBuffer[23]}");
            Console.WriteLine($"Left Trigger State: {inputBuffer[24]}");
            Console.WriteLine($"Left Trigger Force Exerted In Range (mode 2): {inputBuffer[25]}");
            Console.WriteLine($"Strength of Effect Near Release State (requires modes 4 & 20): {inputBuffer[26]}");
            Console.WriteLine($"Strength of Effect Near Middle State (requires modes 4 & 20): {inputBuffer[27]}");
            Console.WriteLine($"Strength of Effect At Pressed State (requires modes 4 & 20): {inputBuffer[28]}");
            Console.WriteLine($"Effect Actuation Frequency (Hz requires modes 4 & 20): {inputBuffer[31]}");
            Console.WriteLine();
            Console.WriteLine($"Main Motor: {inputBuffer[37]}");
            Console.WriteLine($"Trigger Effects: {inputBuffer[37]}");
            Console.WriteLine($"Internal Speaker Volume (0-7): {inputBuffer[38]}");
            Console.WriteLine();
            Console.WriteLine($"LED Flags: {Convert.ToString(inputBuffer[39], 2).PadLeft(8, '0')}");
            Console.WriteLine($"Pulse Option: {inputBuffer[42]}");
            Console.WriteLine($"LED Brightness: {inputBuffer[43]}");
            Console.WriteLine($"Touchbar Bottom Strip: {Convert.ToString(inputBuffer[44], 2).PadLeft(8, '0')}");
            Console.WriteLine($"Touchbar Red  : {inputBuffer[45]}");
            Console.WriteLine($"Touchbar Green: {inputBuffer[46]}");
            Console.WriteLine($"Touchbar Blue : {inputBuffer[47]}");
            //connection.Flush();
        }

        public void PrintRGB() {
            connection.Read(inputBuffer);
            Console.WriteLine($"Touchbar Red  : {inputBuffer[45]}");
            Console.WriteLine($"Touchbar Green: {inputBuffer[46]}");
            Console.WriteLine($"Touchbar Blue : {inputBuffer[47]}");
            // connection.Flush();
        }

        public void SetRGB(float red, float green, float blue) {
            outputBuffer[0] = 49;
            outputBuffer[1] = 0xff;
            outputBuffer[2] = 0xff - 8;
            outputBuffer[39] = 2;
            outputBuffer[43] = 0x02;
            outputBuffer[45] = 0;  // Convert.ToByte(red);
            outputBuffer[46] = 128;// Convert.ToByte(green);
            outputBuffer[47] = 0;  // Convert.ToByte(blue);
            connection.Write(outputBuffer);
        }

        public void PrintReportTypes() {
            var description = connection.Device.GetReportDescriptor();
            var receiver = description.CreateHidDeviceInputReceiver();
            var items = description.DeviceItems;
            foreach (var item in items) {
                //item.
            }
            var reports = description.Reports;
            foreach (var report in reports) {
                Console.WriteLine($"Report ID: {report.ReportID} Type: {report.ReportType.ToString()} Length: {report.Length}");
            }
        }
    }
}
