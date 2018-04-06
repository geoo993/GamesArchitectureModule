using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WareHouse3
{
    
    //-----------------------------------------------------------------------------
    // IState
    //
    // Interface for the State design pattern
    //-----------------------------------------------------------------------------
    public abstract class State
    {

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public State()
        {
        }

        //-----------------------------------------------------------------------------
        // Enter() – This is called when the state is first entered.
		// We’ll use this for initializing the state before executing any behavioural code associated with it.
        //-----------------------------------------------------------------------------
        public abstract void Enter(ref Object parent);

        //-----------------------------------------------------------------------------
        // Update() – This is the function where all the magic happens.
        // Every time the FSM is updated, it calls this function on the current state.
        //-----------------------------------------------------------------------------
        public abstract void Update(ref Object parent, GameTime gameTime);
        
        
        public abstract void Draw(ref Object parent, SpriteBatch spriteBatch, Vector2 screenCenter);

        //-----------------------------------------------------------------------------
        // Exit() – This is called when the FSM is about to move to a different state. 
		// This cleans up everything that we’ve initialised in Enter()
        //-----------------------------------------------------------------------------
        public abstract void Exit(ref Object parent);
        
        //-----------------------------------------------------------------------------
        // Set the name of the current state
        //-----------------------------------------------------------------------------
        public string Name { get; set; }
 
    }
}
