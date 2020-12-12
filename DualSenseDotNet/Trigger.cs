using System;
using System.Collections.Generic;
using System.Text;

namespace DualSenseDotNet {
    public class Trigger {
        private float position;
        /// <summary> 0-1 value for how pressed in the  <see cref="Trigger"/> is. </summary>
        public float Position {
            get => position;
            internal set {
                if (position == value)
                    return;

                position = value;
                PositionChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Invoked when the state of the <see cref="Trigger"/> changes. <br/>
        /// Contains the new state of the <see cref="Trigger"/>.
        /// </summary>
        public event Action<float> PositionChanged;

        public enum EffectType : byte {
            NoResistance = 0,
            ContinuousResistance = 1,
            SectionResistance = 2,
            Extended = 38,
            Calibrate = 252
        }
    }
}
