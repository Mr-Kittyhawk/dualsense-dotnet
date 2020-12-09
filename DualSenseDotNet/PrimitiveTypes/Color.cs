using System;

namespace DualSenseDotNet.PrimitiveTypes {
    public struct Color {
        byte Red;
        byte Green;
        byte Blue;


        // TODO Test me
        public Color(float red, float green, float blue) {
            Red = Convert.ToByte(red);
            Green = Convert.ToByte(green);
            Blue = Convert.ToByte(blue);
        }

        // TODO Test me
        public Color(int red, int green, int blue) {
            Red = Convert.ToByte(red);
            Green = Convert.ToByte(green);
            Blue = Convert.ToByte(blue);
        }
    }
}
