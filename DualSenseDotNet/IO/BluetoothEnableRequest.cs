using System;
using DualSenseDotNet.PrimitiveTypes;

namespace DualSenseDotNet.IO {
    /// <summary> Requests that the controller communicate over bluetooth with us. </summary>
    internal class BluetoothEnableRequest : HIDReport<BluetoothEnableRequest> {
        internal override byte ID => 5;
        private static byte[] cachedMessage;

        internal BluetoothEnableRequest() {
            if(cachedMessage is null) {
                cachedMessage = new byte[64];
                cachedMessage[0] = ID;
            }
        }

        internal override HIDReport<BluetoothEnableRequest> Deserialize(byte[] rawMessage, ConnectionType connectionType) {
            throw new NotImplementedException();
        }

        internal override byte[] Serialize(ref byte[] outputBuffer, ConnectionType connectionType) => cachedMessage;
    }
}
