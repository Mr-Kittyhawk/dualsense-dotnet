using System;
using System.Numerics;
using System.Collections;
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
        public void PollState() => ConsumeInputReport(controllerLink.ReadRaw());

        /// <summary> Updates the state of this class from an input report. </summary>
        public void ConsumeInputReport(byte[] inputReport) {
            switch (inputReport[0]) {
                case 0: // the input report is blank
                    return;
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
            float x = (inputReport[1] - 128) / 127f;
            float y = ((inputReport[2] - 127) * -1f) / 127f;

            LeftStick.Position = new Vector2(x, y);

            x = (inputReport[3] - 128) / 127f;
            y = ((inputReport[4] - 127) * -1f) / 127f;

            RightStick.Position = new Vector2(x, y);

            LeftTrigger.Position = inputReport[5] / 255f;
            RightTrigger.Position = inputReport[6] / 255f;

            var faceButtonState = new BitArray(new byte[] { inputReport[8] });
            if (faceButtonState[3])
                DPad.Direction = DirectionalPad.DirectionEnum.None;
            else {
                if(faceButtonState[0] && faceButtonState[1] && faceButtonState[2])
                    DPad.Direction = DirectionalPad.DirectionEnum.UpLeft;
                else if(faceButtonState[1] && faceButtonState[2])
                    DPad.Direction = DirectionalPad.DirectionEnum.Left;
                else if (faceButtonState[0] && faceButtonState[2])
                    DPad.Direction = DirectionalPad.DirectionEnum.DownLeft;
                else if (faceButtonState[2])
                    DPad.Direction = DirectionalPad.DirectionEnum.Down;
                else if (faceButtonState[0] && faceButtonState[1])
                    DPad.Direction = DirectionalPad.DirectionEnum.DownRight;
                else if (faceButtonState[1])
                    DPad.Direction = DirectionalPad.DirectionEnum.Right;
                else if (faceButtonState[0])
                    DPad.Direction = DirectionalPad.DirectionEnum.UpRight;
                else
                    DPad.Direction = DirectionalPad.DirectionEnum.Up;
            }
            Square.IsDown = faceButtonState[4];
            Cross.IsDown = faceButtonState[5];
            Circle.IsDown = faceButtonState[6];
            Triangle.IsDown = faceButtonState[7];

            var additionalButtonGroup = new BitArray(new byte[] { inputReport[9] });
            LeftShoulder.IsDown = additionalButtonGroup[0];
            RightShoulder.IsDown = additionalButtonGroup[1];
            ShareButton.IsDown = additionalButtonGroup[4];
            MenuButton.IsDown = additionalButtonGroup[5];
            LeftStick.Button.IsDown = additionalButtonGroup[6];
            RightStick.Button.IsDown = additionalButtonGroup[7];

            var centerButtonGroup = new BitArray(new byte[] { inputReport[10] });
            PlayStationButton.IsDown = centerButtonGroup[0];
            MicrophoneButton.IsDown = centerButtonGroup[2];
        }

        private void ConsumeBluetoothInputReport(byte[] inputReport) {
            float x = (inputReport[2] - 128) / 127f;
            float y = ((inputReport[3] - 127) * -1f) / 127f;

            LeftStick.Position = new Vector2(x, y);

            x = (inputReport[4] - 128) / 127f;
            y = ((inputReport[5] - 127) * -1f) / 127f;

            RightStick.Position = new Vector2(x, y);

            LeftTrigger.Position = inputReport[6] / 255f;
            RightTrigger.Position = inputReport[7] / 255f;

            var faceButtonState = new BitArray(new byte[] { inputReport[9] });
            if (faceButtonState[3])
                DPad.Direction = DirectionalPad.DirectionEnum.None;
            else {
                if (faceButtonState[0] && faceButtonState[1] && faceButtonState[2])
                    DPad.Direction = DirectionalPad.DirectionEnum.UpLeft;
                else if (faceButtonState[1] && faceButtonState[2])
                    DPad.Direction = DirectionalPad.DirectionEnum.Left;
                else if (faceButtonState[0] && faceButtonState[2])
                    DPad.Direction = DirectionalPad.DirectionEnum.DownLeft;
                else if (faceButtonState[2])
                    DPad.Direction = DirectionalPad.DirectionEnum.Down;
                else if (faceButtonState[0] && faceButtonState[1])
                    DPad.Direction = DirectionalPad.DirectionEnum.DownRight;
                else if (faceButtonState[1])
                    DPad.Direction = DirectionalPad.DirectionEnum.Right;
                else if (faceButtonState[0])
                    DPad.Direction = DirectionalPad.DirectionEnum.UpRight;
                else
                    DPad.Direction = DirectionalPad.DirectionEnum.Up;
            }
            Square.IsDown = faceButtonState[4];
            Cross.IsDown = faceButtonState[5];
            Circle.IsDown = faceButtonState[6];
            Triangle.IsDown = faceButtonState[7];

            var additionalButtonGroup = new BitArray(new byte[] { inputReport[10] });
            LeftShoulder.IsDown = additionalButtonGroup[0];
            RightShoulder.IsDown = additionalButtonGroup[1];
            ShareButton.IsDown = additionalButtonGroup[4];
            MenuButton.IsDown = additionalButtonGroup[5];
            LeftStick.Button.IsDown = additionalButtonGroup[6];
            RightStick.Button.IsDown = additionalButtonGroup[7];

            var centerButtonGroup = new BitArray(new byte[] { inputReport[11] });
            PlayStationButton.IsDown = centerButtonGroup[0];
            MicrophoneButton.IsDown = centerButtonGroup[2];
        }

        public void SetRGB(float red, float green, float blue) => controllerLink.SetRGB(red, green, blue);

    }
}
