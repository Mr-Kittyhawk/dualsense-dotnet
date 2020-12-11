using System;
using System.Collections.Generic;
using System.Text;

namespace DualSenseDotNet {
    public class ControllerDebug {
        /// <summary> Invoked whenever an input report comes in from the controller. </summary>
        public event Action<byte[]> RawInputReportReceived;

        internal ControllerDebug(ControllerConnection connection) {
            connection.InputReportReceived += () => RawInputReportReceived?.Invoke(connection.inputBuffer);
        }
    }
}
