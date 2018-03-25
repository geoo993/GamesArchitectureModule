using System;

namespace WareHouse3
{
    public class JumpState
    {
        public enum Mode
        {
            first,
            second,
            grounded,
        }

        public Mode mode { get; set;  }
        
        public bool IsFirstJump {
            get {
                return mode == Mode.first;
            }
        }
        
        public bool IsSecondJump {
            get {
                return mode == Mode.second;
            }
        }
        
        public bool IsGrounded {
            get {
                return mode == Mode.grounded;
            }
        }
        
        public JumpState() {
            mode = Mode.grounded;
        }
    }
}
