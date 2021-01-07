
namespace DualSenseDotNet {
    public static class ByteExtensions {

        /// <summary> Converts a <see cref="byte"/> to a <see cref="float"/> with a value between 0 and 1. </summary>
        public static float ToNormalizedFloat(this byte @byte) => @byte / 255f;
    }
}
