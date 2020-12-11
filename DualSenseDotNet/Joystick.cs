using System.Numerics;
using System;

namespace DualSenseDotNet {
    public class Joystick {
        private Vector2 position;

        /// <summary> 0-1 location of the joystick on each axis. </summary>
        public Vector2 Position { 
            get => position;
            internal set {
                position = value;
                Moved?.Invoke(value);
            }
        }

        /// <summary> The button that activates wen you "click" the <see cref="Joystick"/> in. </summary>
        public Button Button;

        /// <summary>
        /// Invoked when the state of the joystick changes. <br/>
        /// Contains the new state of the joystick.
        /// </summary>
        public event Action<Vector2> Moved;
    }
}
