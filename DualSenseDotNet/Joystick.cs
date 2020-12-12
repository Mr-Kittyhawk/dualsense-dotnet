using System.Numerics;
using System;

namespace DualSenseDotNet {
    public class Joystick {
        private Vector2 position;

        /// <summary> 0-1 location of the <see cref="Joystick"/> on each axis. </summary>
        public Vector2 Position {
            get => position;
            internal set {
                if (position == value)
                    return;

                position = value;
                if (Math.Abs(value.X) > DeadZone || Math.Abs(value.Y) > DeadZone)
                    PositionChanged?.Invoke(value);
            }
        }

        public float DeadZone;

        /// <summary>
        /// Invoked when the state of the <see cref="Joystick"/> changes. <br/>
        /// Contains the new state of the <see cref="Joystick"/>.
        /// </summary>
        public event Action<Vector2> PositionChanged;

        /// <summary> The button that activates when you "click" the <see cref="Joystick"/> in. </summary>
        public Button Button = new Button();

        public Joystick(float deadZone = 0.05f) {
            DeadZone = deadZone;
        }
    }
}
