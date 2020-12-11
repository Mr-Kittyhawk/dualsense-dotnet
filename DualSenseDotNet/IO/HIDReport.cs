using DualSenseDotNet.PrimitiveTypes;

namespace DualSenseDotNet.IO {
    /// <summary> Data transfer class for Human Interface Devices. </summary>
    abstract internal class HIDReport<T> {
        /// <summary> Header for allowing receiving devices to know what kind of report this is. </summary>
        abstract internal byte ID { get; }


        /// <summary> Converts this <see cref="HIDReport{T}"/> to a raw message that can be sent over the network to a <see cref="DualSenseController"/>. </summary>
        /// <remarks> Different connection types require different data layouts inside the buffer. </remarks>
        internal abstract byte[] Serialize(ref byte[] outputBuffer, ConnectionType connectionType);

        internal abstract HIDReport<T> Deserialize(byte[] rawMessage, ConnectionType connectionType);
    }
}
