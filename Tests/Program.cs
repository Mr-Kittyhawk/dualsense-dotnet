using System;
using System.Linq;
using System.Collections.Generic;

namespace Tests {
    class Program {
        static void Main(string[] args) {
            var connectedControllers = DualSenseDotNet.Basic.GetConnectedControllers();
            if (connectedControllers.Count() == 0) {
                Console.WriteLine("No controllers detected. Exiting...");
                return;
            }
                

            var controller = connectedControllers.First();
            Console.WriteLine($"Controller {controller.SerialNumber} connected!");
            char lastKey = 'c';


            while(true) {
                //controller.PollState();
                //controller.PrintButtonStates();

                lastKey = Console.ReadKey().KeyChar;
            }
        }
        
    }
}
