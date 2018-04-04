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
        
        public enum Event {
            didStayGrounded,
            didStartMoving,
            didStartFalling,
            didStartJumping
        }
        
        public class State {
            public Mode mode = Mode.grounded;
            
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

        public State state { get; private set; }
         
        public MotionState() {
            state = new State();
        }
        
        public void Process(Event even) {
            switch (even) {
            case Event.didStayGrounded:
                 state.mode = Mode.grounded;
                 break;
            case Event.didStartMoving:
                 state.mode = Mode.moving;
                 break;
            case Event.didStartFalling:
                 state.mode = Mode.falling;
                 break;
            case Event.didStartJumping:
                 state.mode = Mode.jumping;
                 break;
            default:
                 break;
            
            }
        }
        
    }
}
