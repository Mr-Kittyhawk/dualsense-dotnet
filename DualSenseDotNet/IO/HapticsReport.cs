using System;
using System.Collections.Generic;
using System.Text;
using DualSenseDotNet.PrimitiveTypes;

namespace DualSenseDotNet.IO {
    internal class HapticsReport : HIDReport<HapticsReport> {
        internal override byte ID { get => 49; }

        internal override HIDReport<HapticsReport> Deserialize(byte[] rawMessage, ConnectionType connectionType) {
            if (rawMessage[0] != ID)
                throw new System.ArgumentException("Message is of the wrong type!");

            return null;
        }

        internal override byte[] Serialize(ref byte[] outputBuffer, ConnectionType connectionType) {
            throw new NotImplementedException();
        }
    }
}
