using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XylophoneGame
{
    public class StateMachine
    {
        
        private Object Parent;

        private float UpdateWaitMilliseconds;
        private TimeSpan PreviousGameTime;
        
        protected State CurrentState;
        protected State PreviousState;

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public StateMachine(Object parent, float updateWaitMilliseconds, State initialState)
        {
            Parent = parent;

            UpdateWaitMilliseconds = updateWaitMilliseconds;
            PreviousGameTime = TimeSpan.Zero;
            
            CurrentState = initialState;
            PreviousState = null;

        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Destroy()
        {
            Parent = null;
            CurrentState = null;
            PreviousState = null;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public State GetState()
        {
            return CurrentState;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public State GetPreviousState()
        {
            return PreviousState;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetUpdateWaitMilliseconds(float milliseconds)
        {
            UpdateWaitMilliseconds = milliseconds;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void ClearCurrentState()
        {
            CurrentState = null;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetState(State state)
        {
            if (state == null)
            {
                return;
            }

            if (state == CurrentState)
            {
                return;
            }

            PreviousState = CurrentState;

            if (CurrentState != null)
                CurrentState.Exit(ref Parent);

            CurrentState = state;
            CurrentState.Enter(ref Parent);
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void RevertToPreviousState()
        {
            SetState(PreviousState);
            PreviousState = null;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Update(GameTime gameTime)
        {

            if (CurrentState != null)
            {
                CurrentState.Update(ref Parent, gameTime);
            }

            PreviousGameTime = gameTime.TotalGameTime;
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Draw(SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {

            if (CurrentState != null)
            {
                CurrentState.Draw(ref Parent, spriteBatch, screenSafeArea);
            }
        }

    
    }
}
