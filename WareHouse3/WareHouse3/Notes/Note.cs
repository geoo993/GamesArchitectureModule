using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace WareHouse3
{
    public class Note: Tile
    {
        /// <summary>
        /// box border 
        /// </summary>
        private PrimitiveLine BoxBorder;
        public bool IsBorderEnabled { get; set; }
        
        
        /// <summary>
        /// The state of the Note.
        /// </summary>
        protected NoteStates State;
        public string NoteName { get; set; }
        
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public NoteStates GetState()
        {
            return State;
        }

        public Note(String name, Vector2 position, int width, int height, float speed, float jump, float mass, Color color, SoundEffect note = null, Texture2D texture = null, TileCollision collision = TileCollision.Passable, NoteStates state = NoteStates.EMPTY)
        : base(name, position, width, height, speed, jump, mass, color, note, texture, collision)
        {
            this.BoxBorder = new PrimitiveLine(Device.graphicsDevice, BorderColor);
            this.IsBorderEnabled = false;
            this.HasFSM = true;
            this.FSM = new StateMachine(this, 0.0f, null);
            this.SetState(state);
            this.HasTexture = true;
        }
    
        public override void UpdatePosition(GameTime gameTime, Vector2 mapSize)
        {
            base.UpdatePosition(gameTime, mapSize);

            if (IsBorderEnabled) {
                BoxBorder.CreateBox(Position - Origin, Position + Origin);
            }
        }
        
        
        /// <summary>
        /// texture area of the box
        /// </summary>
        protected override Texture2D CreateTexture() {
            base.CreateTexture();
            
            Texture2D rectangle = new Texture2D(Device.graphicsDevice, Width, Height, false, SurfaceFormat.Color);
            
            Color[] colorData = new Color[Width * Height];
            for (int i = 0; i < Width * Height; i++)
                colorData[i] = Color;
            rectangle.SetData<Color>(colorData);
            return rectangle;
        }
        
        /// <summary>
        /// Determines if a box intersects a rectangle.
        /// </summary>
        /// <returns>True if the circle and rectangle overlap. False otherwise.</returns>
        public override bool Intersects(Rectangle rectangle)
        {
            base.Intersects(rectangle);
            
            // Get the rectangle half width and height
            float rW1 = (Width) / 2 ;
            float rH1 = (Height) / 2;
            
            float rW2 = (rectangle.Width) / 2 ;
            float rH2 = (rectangle.Height) / 2;
            
            Rectangle rec1 = new Rectangle((int)(BoundingRectangle.X - rW1), (int)(BoundingRectangle.Y - rH1), BoundingRectangle.Width, BoundingRectangle.Height);
            Rectangle rec2 = new Rectangle((int)(rectangle.X - rW2), (int)(rectangle.Y - rH2), rectangle.Width, rectangle.Height);
            
            return rec1.Intersects(rec2);
        }
        
        
        /// <summary>
        /// Render  box with sprite batch.
        /// </summary>
        public override void Render(SpriteBatch spriteBatch) {
            
            base.Render(spriteBatch);

            if (IsBorderEnabled)
            {
                BoxBorder.Render(spriteBatch, 2.0f, this.BorderColor * Opacity);
            }

        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetState(NoteStates state)
        {
            switch (state)
            {
                case NoteStates.EMPTY:
                    FSM.SetState(NoteStatesEmpty.Instance());
                    break;
                case NoteStates.ENABLED:
                    FSM.SetState(NoteStatesEnabled.Instance());
                    break;
                case NoteStates.DISABLED:
                    FSM.SetState(NoteStatesDisabled.Instance());
                    break;
            }
            State = state;
        }
        
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Destroy()
        {
            base.Destroy();
        }
        
    }
}

