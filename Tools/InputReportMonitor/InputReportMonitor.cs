using System;
using System.Linq;

namespace InputReportMonitor {
    /// <summary> A debug tool that displays the raw stream of data coming in from a connected controller. </summary>
    class InputReportMonitor {
        private static bool drawing;

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
            controller.Debug.RawInputReportReceived += rawBuffer => PrintRawHIDReport(rawBuffer, 5);

            char lastKey = 'c';
            while (lastKey != ' ') {
                lastKey = Console.ReadKey().KeyChar;
            }
        }

        /// <param name="startingRow"> The line of the output console to start drawing the report on. </param>
        private static void PrintRawHIDReport(byte[] rawBuffer, uint startingRow = 0u) {
            if (drawing)
                return;

            drawing = true;
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, (int)startingRow);

            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine($"|                        Input Report {rawBuffer[0].ToString().PadLeft(3, '0')}                         |");
            Console.WriteLine("-------------------------------------------------------------------");
            for (int i = 1; i < rawBuffer.Length; i++) {
                if (i % 8 == 1)
                    Console.Write("| ");

                Console.Write($"{i.ToString().PadLeft(3, '0')}:{rawBuffer[i].ToString().PadLeft(3, '0')}");

                if (i % 8 == 0)
                    Console.Write(" |\n");
                else if (i == rawBuffer.Length - 1) {
                    for (int j = Console.CursorLeft; j < 65; j++)
                        Console.Write(' ');
                    Console.Write(" |\n");
                }
                else
                    Console.Write(' ');
            }
            Console.WriteLine("-------------------------------------------------------------------");
            drawing = false;
        }
    }
}
