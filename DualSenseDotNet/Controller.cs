using System;
using System.Collections.Generic;
using System.Text;
using HidSharp;
using DualSenseDotNet.PrimitiveTypes;

namespace DualSenseDotNet {

    /// <summary> Represents an individual Sony Dualsense gamepad. </summary>
    public class Controller {
        public int SerialNumber;
        public string SystemPath;

        public ConnectionType Connection { get => controllerLink.ConnectionType; }
        private ControllerConnection controllerLink;

        public Button LeftArrow;
        public Button RightArrow;
        public Button UpArrow;
        public Button DownArrow;

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
        public Button PlaystationButton;
        public Button MicrophoneButton;

        #region Construction
        internal Controller(HidStream connectionStream) {
            controllerLink = new ControllerConnection(connectionStream);
            controllerLink.InputReportReceived += () => PollState();

            Cross = new Button();
            Square = new Button();
            Triangle = new Button();
            Circle = new Button();
        }
        #endregion Construction

        /// <summary> Updates the state of the <see cref="Controller"/> class instance with the newest data from the physical device. </summary>
        public void PollState() {
            var inputReport = controllerLink.Read();

            Cross.IsDown = inputReport.CrossPressed;
            Square.IsDown = inputReport.SquarePressed;
            Triangle.IsDown = inputReport.TrianglePressed;
            Circle.IsDown = inputReport.CirclePressed;
            //PrintButtonStates();
           // return controllerLink.inputBuffer;
        }

        public void PrintJoystickInfo() {
            // Console.WriteLine($"Left Joystick ({report.LeftStickPosition.X}, {report.LeftStickPosition.Y})");
            // Console.WriteLine($"Right Joystick ({report.RightStickPosition.X}, {report.RightStickPosition.Y})");

            //Console.WriteLine($"Left Trigger : {report.LeftTriggerPosition}");
            //Console.WriteLine($"Right Trigger: {report.RightTriggerPosition}");
        }

        public void PrintButtonStates() {
            Console.WriteLine($"Square: {(Square.IsDown ? '1' : '0')} Trianle: {(Triangle.IsDown ? '1' : '0')} Circle: {(Circle.IsDown ? '1' : '0')} Cross: {(Cross.IsDown ? '1' : '0')}");
        }

        public void PrintState() {
            controllerLink.PrintStreamContents();
        }
        public void PrintRGB() {
            controllerLink.PrintRGB();
        }
        public void PrintReportTypes() => controllerLink.PrintReportTypes();
        public void SetRGB(float red, float green, float blue) => controllerLink.SetRGB(red, green, blue);

    }
}
