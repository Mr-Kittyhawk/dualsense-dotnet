using System;
using System.Collections.Generic;
using System.Text;

namespace DualSenseDotNet {
    /// <summary> Arrows on the <see cref="DualSenseController"/>. </summary>
    public class DirectionalPad {

        private DirectionEnum direction;
        public DirectionEnum Direction {
            get => direction;
            internal set {
                if (direction == value)
                    return;

                direction = value;
                directionLastChangedTime = Environment.TickCount;
                DirectionChanged?.Invoke(value);
            }
        }


        public event Action<DirectionEnum> DirectionChanged;
        private int directionLastChangedTime;

        /// <summary>
        /// Duration the current arrow has been held down for in seconds. <br/
        /// Returns <see cref="DirectionEnum.None"/> if no direction is currently held down.
        /// </summary
        public float DirectionHeldFor { get => direction != DirectionEnum.None? (Environment.TickCount - directionLastChangedTime) / 1000f : 0f; }

        public enum DirectionEnum {
            None = 0,
            Up = 2,
            UpRight = 3,
            Right = 1,
            DownRight = 4,
            Down = 5,
            DownLeft = 6,
            Left = -1,
            UpLeft = 7
        }
    }
}
