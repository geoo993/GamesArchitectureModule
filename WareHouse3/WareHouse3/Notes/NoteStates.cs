using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WareHouse3
{
   
    //-----------------------------------------------------------------------------
    // NoteStates
    //
    // States of a Notes
    //-----------------------------------------------------------------------------
    public enum NoteStates
    {
        EMPTY = 0,  // mean that it is just a tile or object that does not look like a note
        DISABLED,   // means that it is payable but disable/turned off
        ENABLED     // means tthat it is playable
    }

    //-----------------------------------------------------------------------------
    // NoteStatesEmpty
    //
    // 
    //-----------------------------------------------------------------------------
    public class NoteStatesEmpty : State
    {
        // Singleton
        private static NoteStatesEmpty mInstance = null;
        public static NoteStatesEmpty Instance()
        {
            if (mInstance == null)
                mInstance = new NoteStatesEmpty();
            return mInstance;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public NoteStatesEmpty()
            : base()
        {
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Enter(ref Object owner)
        {
            Tile tile = (Tile)owner;
            Note note = (Note)tile;
            note.HasTexture = false;
            //Debug.Print("Enter: "+note.Name +" States Empty");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, GameTime gameTime)
        {
            Tile tile = (Tile)owner;
           
            Note note = (Note)tile;
            
           
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------   
        public override void Draw(ref Object owner, SpriteBatch spriteBatch, Vector2 screenCenter)
        {
            
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Tile tile = (Tile)owner;
            Note note = (Note)tile;
            //Debug.Print("Exit: "+note.Name +" States Empty");
        }
    }


    //-----------------------------------------------------------------------------
    // NoteStatesEnabled
    //
    // 
    //-----------------------------------------------------------------------------
    public class NoteStatesEnabled : State
    {
        // Singleton
        private static NoteStatesEnabled mInstance = null;
        public static NoteStatesEnabled Instance()
        {
            if (mInstance == null)
                mInstance = new NoteStatesEnabled();
            return mInstance;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public NoteStatesEnabled()
            : base()
        {
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Enter(ref Object owner)
        {
             Tile tile = (Tile)owner;
             Note note = (Note)tile;
             //note.HasTexture = true;
             //note.Color = Color.White;
            
             if (note.NoteSound != null) {
                note.NoteSound.Play();
             }
            
             //Debug.Print("Enter: "+note.Name +" States Enabled");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, GameTime gameTime)
        {
            Tile tile = (Tile)owner;
            
            Note note = (Note)tile;

        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------   
        public override void Draw(ref Object owner, SpriteBatch spriteBatch, Vector2 screenCenter)
        {
            
        }
      
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Tile tile = (Tile)owner;
            Note note = (Note)tile;
            //note.HasTexture = false;
            //note.Color = note.InitialColor;
            //Debug.Print("Exit: "+note.Name +" States Enabled");
        }
    }


    //-----------------------------------------------------------------------------
    // NoteStatesDisabled
    //
    //
    //-----------------------------------------------------------------------------
    public class NoteStatesDisabled : State
    {
        // Singleton
        private static NoteStatesDisabled mInstance = null;
        public static NoteStatesDisabled Instance()
        {
            if (mInstance == null)
                mInstance = new NoteStatesDisabled();
            return mInstance;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public NoteStatesDisabled()
            : base()
        {
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Enter(ref Object owner)
        {
            Tile tile = (Tile)owner;
            Note note = (Note)tile;
            //Debug.Print("Enter: "+note.Name +" States Disabled");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, GameTime gameTime)
        {
            Tile tile = (Tile)owner;
            Note note = (Note)tile;
            //Debug.Log(Debug.Flags.DEBUG_NPC, "[ {0} ] GameNPCStateIdle.Update()", npc.GetGameObjectType());
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------   
        public override void Draw(ref Object owner, SpriteBatch spriteBatch, Vector2 screenCenter)
        {
            
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Tile tile = (Tile)owner;
            Note note = (Note)tile;
            //Debug.Print("Exit: "+note.Name +" States Disabled");
        }
    }

}
