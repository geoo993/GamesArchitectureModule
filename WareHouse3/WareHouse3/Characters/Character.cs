using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// https://www.youtube.com/watch?v=ZLxIShw-7ac

namespace WareHouse3
{
    public class Character: Box
    {
    
        public Character(Vector2 position, int width, int height, float speed, float jump, Color color, Texture2D texture = null)
        : base(position, width, height, speed, jump, color, texture)
        {
            
        }
        
        protected override void LeftMovement(ButtonAction buttonState, Vector2 amount)
        {
          
            if (buttonState == ButtonAction.DOWN)
            {
               
            }
        }
        
        protected override void RightMovement(ButtonAction buttonState, Vector2 amount)
        {
           
            if (buttonState == ButtonAction.DOWN)
            {
               
            } 
        }
       
        protected override void UpMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                
            }
        }
        
        
        protected override void DownMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
               
            }
        }
        
        protected override void JumpMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
               
            }
        }
        
        protected override void RotateForward(ButtonAction buttonState, Vector2 amount)
        {
           
        }
        
        protected override void RotateBackward(ButtonAction buttonState, Vector2 amount)
        {
          
        }
        
        public override void UpdatePosition(GameTime gameTime, Vector2 screenSize)
        {    
            Position.X += MoveSpeed; 
            Position.Y += MoveSpeed; 
			base.UpdatePosition(gameTime, screenSize);
        }
     

        public override bool Intersects(Rectangle rectangle)
        {
            return base.Intersects(rectangle);
        }
        
        public override void Render(SpriteBatch spriteBatch) 
        {

			base.Render(spriteBatch);
        }
        
    }
}
