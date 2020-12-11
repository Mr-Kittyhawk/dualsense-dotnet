using System.Numerics;
using System;
using DualSenseDotNet.PrimitiveTypes;

namespace DualSenseDotNet.IO {
    /// <summary> HID Input Report</summary>
    internal class InputReport : HIDReport<InputReport> {
        internal override byte ID => 1;

        internal Vector2 LeftStickPosition;
        internal Vector2 RightStickPosition;
        internal float LeftTriggerPosition;
        internal float RightTriggerPosition;

        internal bool CrossPressed;
        internal bool SquarePressed;
        internal bool TrianglePressed;
        internal bool CirclePressed;

        internal override byte[] Serialize(ref byte[] outputBuffer, ConnectionType connectionType) {
            throw new System.NotImplementedException();
        }

        internal override HIDReport<InputReport> Deserialize(byte[] rawMessage, ConnectionType connectionType) {
            // Convert sticks to signed range
            LeftStickPosition.X = rawMessage[2] - 128;
            LeftStickPosition.Y = (rawMessage[3] - 127) * -1f;
            RightStickPosition.X = rawMessage[4] - 128;
            RightStickPosition.Y = (rawMessage[5] - 127) * -1f;

            LeftTriggerPosition = rawMessage[6];
            RightTriggerPosition = rawMessage[7];

            CrossPressed = Convert.ToBoolean(rawMessage[8]);
            SquarePressed = Convert.ToBoolean(rawMessage[9]);
            TrianglePressed = Convert.ToBoolean(rawMessage[10]);
            CirclePressed = Convert.ToBoolean(rawMessage[11]);
            //PrintRawState(rawMessage);
            return this;
            /*
            // Convert trigger to unsigned range
            

            // Buttons
            buttonsAndDpad = rawMessage[9] & 0xF0;
            buttonsA = rawMessage[10];
            buttonsB = rawMessage[11];

            // Dpad
            switch (rawMessage[0x07] & 0x0F) {
                // Up
                case 0x0:
                    buttonsAndDpad |= DS5W_ISTATE_DPAD_UP;
                    break;
                // Down
                case 0x4:
                    buttonsAndDpad |= DS5W_ISTATE_DPAD_DOWN;
                    break;
                // Left
                case 0x6:
                    buttonsAndDpad |= DS5W_ISTATE_DPAD_LEFT;
                    break;
                // Right
                case 0x2:
                    buttonsAndDpad |= DS5W_ISTATE_DPAD_RIGHT;
                    break;
                // Left Down
                case 0x5:
                    buttonsAndDpad |= DS5W_ISTATE_DPAD_LEFT | DS5W_ISTATE_DPAD_DOWN;
                    break;
                // Left Up
                case 0x7:
                    buttonsAndDpad |= DS5W_ISTATE_DPAD_LEFT | DS5W_ISTATE_DPAD_UP;
                    break;
                // Right Up
                case 0x1:
                    buttonsAndDpad |= DS5W_ISTATE_DPAD_RIGHT | DS5W_ISTATE_DPAD_UP;
                    break;
                // Right Down
                case 0x3:
                    buttonsAndDpad |= DS5W_ISTATE_DPAD_RIGHT | DS5W_ISTATE_DPAD_DOWN;
                    break;
            }

            // Copy accelerometer readings
            memcpy(&accelerometer, &rawMessage[0x0F], 2 * 3);

            //TEMP: Copy gyro data (no processing currently done!)
            memcpy(&gyroscope, &rawMessage[0x15], 2 * 3);

            // Evaluate touch state 1
            UINT32 touchpad1Raw = *(UINT32*)(&rawMessage[0x20]);
            touchPoint1.y = (touchpad1Raw & 0xFFF00000) >> 20;
            touchPoint1.x = (touchpad1Raw & 0x000FFF00) >> 8;

            // Evaluate touch state 2
            UINT32 touchpad2Raw = *(UINT32*)(&rawMessage[0x24]);
            touchPoint2.y = (touchpad2Raw & 0xFFF00000) >> 20;
            touchPoint2.x = (touchpad2Raw & 0x000FFF00) >> 8;

            // Evaluate headphone input
            headPhoneConnected = rawMessage[0x35] & 0x01;

            // Trigger force feedback
            leftTriggerFeedback = rawMessage[0x2A];
            rightTriggerFeedback = rawMessage[0x29];

            // Battery
            battery.chargin = (rawMessage[0x35] & 0x08);
            battery.fullyCharged = (rawMessage[0x36] & 0x20);
            battery.level = (rawMessage[0x36] & 0x0F);
            */
        }

        private static bool drawing;

        private static void PrintRawState(byte[] rawBuffer) {
            if (drawing)
                return;
            drawing = true;
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("-------------------------------------------------------------------");
            Console.WriteLine("|                          Input Report                           |");
            Console.WriteLine("-------------------------------------------------------------------");
            Console.Write("| ");
            for (int i = 0; i < Math.Min(rawBuffer.Length, 8); i++) {
                Console.Write($"{i.ToString().PadLeft(3, '0')}:{rawBuffer[i].ToString().PadLeft(3, '0')}");

                if (i == 7 || i == rawBuffer.Length - 1)
                    Console.Write(" |\n");
                else
                    Console.Write(' ');
            }
            for (int i = 9; i < rawBuffer.Length; i++) {
                if (i % 8 == 1)
                    Console.Write("| ");

                Console.Write($"{i.ToString().PadLeft(3, '0')}:{rawBuffer[i].ToString().PadLeft(3, '0')}");

                if (i % 8 == 0)
                    Console.Write(" |\n");
                else if (i == rawBuffer.Length - 1) {
                    for (int j = 0; j < 88 - Console.CursorLeft; j++)
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
