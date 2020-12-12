using System;
using System.Linq;

namespace InputEventMonitor {
    /// <summary> A debug tool for watching what events are being raised from interacting with the controller. </summary>
    class InputEventMonitor {
        static void Main(string[] args) {
            Console.WriteLine("Searching for DualSense controllers...");

            var connectedControllers = DualSenseDotNet.DualSenseDotNet.GetConnectedControllers();
            if (connectedControllers.Count() == 0) {
                Console.WriteLine("No controllers detected. Exiting...");
                return;
            }

            Console.WriteLine($"Found {connectedControllers.Count()} controller{(connectedControllers.Count() > 1 ? "s" : "")}!");

            var controller = connectedControllers.First();
            Console.WriteLine($"Controller {controller.SystemPath} connected");

            controller.DPad.DirectionChanged += newDirection => Console.WriteLine($"{nameof(controller.DPad)} {newDirection}");

            controller.Square.Pressed += () => Console.WriteLine($"{nameof(controller.Square)} pressed");
            controller.Square.Released += () => Console.WriteLine($"{nameof(controller.Square)} released");

            controller.Circle.Pressed += () => Console.WriteLine($"{nameof(controller.Circle)} pressed");
            controller.Circle.Released += () => Console.WriteLine($"{nameof(controller.Circle)} released");

            controller.Triangle.Pressed += () => Console.WriteLine($"{nameof(controller.Triangle)} pressed");
            controller.Triangle.Released += () => Console.WriteLine($"{nameof(controller.Triangle)} released");

            controller.Cross.Pressed += () => Console.WriteLine($"{nameof(controller.Cross)} pressed");
            controller.Cross.Released += () => Console.WriteLine($"{nameof(controller.Cross)} released");

            controller.LeftTrigger.PositionChanged += newPosition => Console.WriteLine($"{nameof(controller.LeftTrigger)} position: {newPosition}");
            controller.RightTrigger.PositionChanged += newPosition => Console.WriteLine($"{nameof(controller.RightTrigger)} position: {newPosition}");

            controller.LeftShoulder.Pressed += () => Console.WriteLine($"{nameof(controller.LeftShoulder)} pressed");
            controller.LeftShoulder.Released += () => Console.WriteLine($"{nameof(controller.LeftShoulder)} released");
            controller.RightShoulder.Pressed += () => Console.WriteLine($"{nameof(controller.RightShoulder)} pressed");
            controller.RightShoulder.Released += () => Console.WriteLine($"{nameof(controller.RightShoulder)} released");

            controller.LeftStick.PositionChanged += newPosition => Console.WriteLine($"{nameof(controller.LeftStick)} position: {newPosition}");
            controller.RightStick.PositionChanged += newPosition => Console.WriteLine($"{nameof(controller.RightStick)} position: {newPosition}");

            controller.LeftStick.Button.Pressed += () => Console.WriteLine($"{nameof(controller.LeftStick)} pressed");
            controller.LeftStick.Button.Released += () => Console.WriteLine($"{nameof(controller.LeftStick)} released");
            controller.RightStick.Button.Pressed += () => Console.WriteLine($"{nameof(controller.RightStick)} pressed");
            controller.RightStick.Button.Released += () => Console.WriteLine($"{nameof(controller.RightStick)} released");

            controller.MenuButton.Pressed += () => Console.WriteLine($"{nameof(controller.MenuButton)} pressed");
            controller.MenuButton.Released += () => Console.WriteLine($"{nameof(controller.MenuButton)} released");
            controller.ShareButton.Pressed += () => Console.WriteLine($"{nameof(controller.ShareButton)} pressed");
            controller.ShareButton.Released += () => Console.WriteLine($"{nameof(controller.ShareButton)} released");

            controller.PlayStationButton.Pressed += () => Console.WriteLine($"{nameof(controller.PlayStationButton)} pressed");
            controller.PlayStationButton.Released += () => Console.WriteLine($"{nameof(controller.PlayStationButton)} released");
            controller.MicrophoneButton.Pressed += () => Console.WriteLine($"{nameof(controller.MicrophoneButton)} pressed");
            controller.MicrophoneButton.Released += () => Console.WriteLine($"{nameof(controller.MicrophoneButton)} released");

            char lastKey = 'c';
            while (lastKey != ' ') {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.WriteLine($"DPad {controller.DPad.Direction} held for {controller.DPad.DirectionHeldFor} seconds");

                lastKey = Console.ReadKey().KeyChar;
            }
        }
    }
}
