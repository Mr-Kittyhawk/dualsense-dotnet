using System;

namespace DualSenseDotNet {

    /// <summary> Represents the physical buttons on the controller. </summary>
    public class Button {
        /// <summary> Is the <see cref="Button"/> currently pressed? </summary>
        public bool IsDown {
            get => isDown;
            internal set {
                if (isDown == value)
                    return;

                isDown = value;
                
                if (value is true) {
                    lastPressedTime = Environment.TickCount;
                    Pressed?.Invoke();
                }
                else
                    Released?.Invoke();
            }
        }

        /// <summary> Duration this button has been held down for in seconds. </summary>
        public float HeldFor { get => IsDown ? (Environment.TickCount - lastPressedTime) * 1000f : 0f; }

        private bool isDown;
        private int lastPressedTime;

        public event Action Pressed;
        public event Action Released;
    }
}
