using System;
using System.Numerics;
using HidSharp;
using DualSenseDotNet.PrimitiveTypes;

namespace DualSenseDotNet {

    public class DualSenseController {

        private ControllerConnection controllerLink;
        public ControllerDebug Debug;

        // public int SerialNumber;
        public string SystemPath;

        public ConnectionType Connection { get => controllerLink.ConnectionType; }

        public DirectionalPad DPad;

        public Button Cross;
        public Button Square;
        public Button Triangle;
        public Button Circle;

        public Button LeftShoulder;
        public Button RightShoulder;

        public Trigger LeftTrigger;
        public Trigger RightTrigger;

        public Joystick LeftStick;
        public Joystick RightStick;

        public Button MenuButton;
        public Button ShareButton;
        public Button PlayStationButton;
        public Button MicrophoneButton;

        #region Construction
        internal DualSenseController(HidStream connectionStream) {

            DPad = new DirectionalPad();
            Cross = new Button();
            Square = new Button();
            Triangle = new Button();
            Circle = new Button();

            LeftShoulder = new Button();
            RightShoulder = new Button();

            LeftTrigger = new Trigger();
            RightTrigger = new Trigger();

            LeftStick = new Joystick();
            RightStick = new Joystick();

            MenuButton = new Button();
            ShareButton = new Button();
            PlayStationButton = new Button();
            MicrophoneButton = new Button();

            controllerLink = new ControllerConnection(connectionStream);
            Debug = new ControllerDebug(controllerLink);

            // SerialNumber = int.Parse(controllerLink.GetSerialNumber());
            SystemPath = controllerLink.GetSystemPath();

            controllerLink.InputReportReceived += ConsumeInputReport;
        }

        ~DualSenseController() {
            controllerLink.InputReportReceived -= ConsumeInputReport;
        }
        #endregion Construction

        /// <summary> Updates the state of this class with the newest data from the physical device. </summary>
        public void PollState() => ConsumeInputReport(controllerLink.inputBuffer);

        /// <summary> Updates the state of this class from an input report. </summary>
        public void ConsumeInputReport(byte[] inputReport) {
            switch (inputReport[0]) {
                case 0:
                case 1:
                    ConsumeWiredInputReport(inputReport);
                    return;
                case 49:
                    ConsumeBluetoothInputReport(inputReport);
                    return;
                default:
                    throw new ArgumentException($"{nameof(DualSenseController)} doesn't know how to process HID input reports of ID: {inputReport[0]}");
            }
        }

        private void ConsumeWiredInputReport(byte[] inputReport) {
            float x = (inputReport[2] - 128) / 255f;
            float y = ((inputReport[3] - 127) * -1f) / 255f;

            LeftStick.Position = new Vector2(x, y);

            x = (inputReport[4] - 128) / 255f;
            y = ((inputReport[5] - 127) * -1f) / 255f;

            RightStick.Position = new Vector2(x, y);

            LeftTrigger.Position = inputReport[6] / 255f;
            RightTrigger.Position = inputReport[7] / 255f;

            Cross.IsDown = Convert.ToBoolean(inputReport[8]);
            Square.IsDown = Convert.ToBoolean(inputReport[9]);
            Triangle.IsDown = Convert.ToBoolean(inputReport[10]);
            Circle.IsDown = Convert.ToBoolean(inputReport[11]);
        }

        private void ConsumeBluetoothInputReport(byte[] inputReport) {

        }

        public void SetRGB(float red, float green, float blue) => controllerLink.SetRGB(red, green, blue);

    }
}
