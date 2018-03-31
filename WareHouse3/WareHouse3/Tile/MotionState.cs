using System;
namespace WareHouse3
{
    
    public class MotionState {
        public enum Mode {
            jumping,
            falling,
            grounded,
            moving,
        } 
        
        public Mode mode;
        public MotionState() {
            mode = Mode.grounded;
        }
        
        public bool IsFalling {
            get {
                return mode == Mode.falling;
            }
        }
        
        public bool IsGrounded {
            get {
                return mode == Mode.grounded;
            }
        }
        
        public bool IsJumping {
            get {
                return mode == Mode.jumping;
            }
        }
        
        public bool IsMoving {
            get {
                return mode == Mode.moving;
            }
        }
    }
}
