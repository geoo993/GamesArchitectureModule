﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace XylophoneGame
{
    public class Note: Tile
    {

        private AnimationPlayer AnimationPlayer;
        
        /// <summary>
        /// box border 
        /// </summary>
        private PrimitiveLine BoxBorder;
        public bool IsBorderEnabled { get; set; }
        
        
        /// <summary>
        /// name of the Note.
        /// </summary>
        public string NoteName { get; set; }
        
        public Note(String name, Vector2 position, int width, int height, float speed, float jump, float mass, Color color, SoundEffect note = null, Texture2D texture = null, TileCollision collision = TileCollision.Passable)
        : base(name, position, width, height, speed, jump, mass, color, note, texture, collision)
        {
            this.BoxBorder = new PrimitiveLine(Device.graphicsDevice, BorderColor);
            this.IsBorderEnabled = false;
            this.HasTexture = true;
        }
    
        public override void UpdatePosition(GameTime gameTime, Vector2 mapSize)
        {
            base.UpdatePosition(gameTime, mapSize);
            
            // move
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
        public override void Draw(SpriteBatch spriteBatch, Rectangle screenSafeArea) {
            
            base.Draw(spriteBatch, screenSafeArea);

            if (IsBorderEnabled)
            {
                BoxBorder.Draw(spriteBatch, 2.0f, this.BorderColor * Opacity);
            }
        }
        
        
        public void Animate(bool doAnimate)
        {
            //if (doAnimate) {
                
            //    AnimationPlayer.PlayAnimation(new Animation(AnimationTexture, Position, 0.4f, 21, 60, Color, false));
            //    //Debug.Print("Activated ");
            //} else {
            //    //Debug.Print("Not Activated " );
            //}
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

