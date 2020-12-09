using System;
using System.Collections.Generic;
using System.Text;

namespace DualSenseDotNet {
    public class Trigger {
        /// <summary> 0-1 value for how pressed in the trigger is. </summary>
        public float Position { get; internal set; }

        public enum EffectType : byte {
            NoResistance = 0,
            ContinuousResistance = 1,
            SectionResistance = 2,
            Extended = 38,
            Calibrate = 252
        }
    }
}
