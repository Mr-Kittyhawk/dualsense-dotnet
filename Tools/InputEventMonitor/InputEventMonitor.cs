using System;
using System.Linq;

namespace InputEventMonitor {
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


            controller.Square.Pressed += () => Console.WriteLine("square pressed");
            controller.Square.Released += () => Console.WriteLine("square released, held for " + controller.Square.HeldFor);

            controller.Circle.Pressed += () => Console.WriteLine("circle pressed");
            controller.Circle.Released += () => Console.WriteLine("circle released, held for " + controller.Circle.HeldFor);

            controller.Triangle.Pressed += () => Console.WriteLine("triangle pressed");
            controller.Triangle.Released += () => Console.WriteLine("triangle released, held for " + controller.Triangle.HeldFor);

            controller.Cross.Pressed += () => Console.WriteLine("cross pressed");
            controller.Cross.Released += () => Console.WriteLine("cross released, held for " + controller.Cross.HeldFor);

            char lastKey = 'c';
            while (lastKey != ' ') {
                lastKey = Console.ReadKey().KeyChar;
            }
        }
    }
}
