using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// https://www.youtube.com/watch?v=ZLxIShw-7ac

namespace WareHouse3
{
    public class Obstacle : Circle
    {

        public Obstacle(Vector2 position, int width, int height, float speed, float jump, Color color, Texture2D texture = null)
        : base(position, width, speed, jump, color, texture)
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

        public override void UpdatePosition(GameTime gameTime, Vector2 mapSize)
        {

            if (BoundingRectangle.Left <= LeftBoundary)
            {
                MoveSpeed.X = Math.Abs(MoveSpeed.X);
            }
            else if (BoundingRectangle.Right >= RightBoundary)
            {
                MoveSpeed.X = -MoveSpeed.X;
            }

            if (BoundingRectangle.Top <= Ceiling)
            {
                MoveSpeed.Y = Math.Abs(MoveSpeed.Y);
            }
            else if (BoundingRectangle.Bottom >= Ground)
            {
                MoveSpeed.Y = -MoveSpeed.Y;
            }

            Position += MoveSpeed;


            base.UpdatePosition(gameTime, mapSize);
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
