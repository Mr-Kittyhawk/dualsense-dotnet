using HidSharp;
using HidSharp.Reports.Input;
using System;
using DualSenseDotNet.IO;
using DualSenseDotNet.PrimitiveTypes;

namespace DualSenseDotNet {
    /// <summary> The data stream over which our <see cref="DualSenseController"/> and the physical Dualsense controller communicate with eachother. </summary>
    internal class ControllerConnection {
        private HidStream connection;
        public ConnectionType ConnectionType { get; private set; }

        /// <summary> for receiving data from the controller. </summary>
        internal byte[] inputBuffer;
        /// <summary> for sending data to the controller. </summary>
        internal byte[] outputBuffer;

        private HidDeviceInputReceiver inputNotifier;

        /// <remarks> 78 bytes on a Sony PS5 controller using bluetooth, 64 if connected by wire. </remarks>
        public readonly int MaxInputReportLength;
        /// <summary> 547 bytes on a Sony PS5 controller using bluetooth, 48 if connected by wire. </summary>
        public readonly int MaxOutputReportLength; // 

        /// <summary> Invoked when we receive a new message from the controller, contains the raw HID Input Report. </summary>
        public event Action<byte[]> InputReportReceived;

        #region Contruction & Destruction
        internal ControllerConnection(HidStream connection) {
            this.connection = connection;
            if (connection.CanRead is false)
                throw new System.Exception();

            HidDevice device = connection.Device;
            inputNotifier = new HidDeviceInputReceiver(device.GetReportDescriptor());

            MaxInputReportLength = device.GetMaxInputReportLength();
            MaxOutputReportLength = device.GetMaxOutputReportLength();

            if (MaxInputReportLength is 78) {
                // enable bluetooth connection
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
        private void RaiseInputReceivedEvent(object @object, EventArgs arguments) => InputReportReceived?.Invoke(inputBuffer);

        /// <summary> Sends a properly formatted set of instructions out to the controller. </summary>
        internal void Write<T>(HIDReport<T> outputReport) {
            outputReport.Serialize(ref outputBuffer, ConnectionType);
            connection.Write(outputBuffer);
            Clear(outputBuffer);
        }

        /// <summary> Reads the most recent message from the controller. </summary>
        internal InputReport Read() {
            connection.Read(inputBuffer);

            var report = new InputReport();

            report.Deserialize(inputBuffer, ConnectionType);

            return report;
        }

        internal string GetSystemPath() => connection.Device.GetFileSystemName();

        /// <summary> 0 fills an array of bytes. </summary>
        private void Clear(byte[] buffer) {
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = 0;
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
    }
}
