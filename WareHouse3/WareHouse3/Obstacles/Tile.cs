using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// https://www.youtube.com/watch?v=ZLxIShw-7ac

namespace WareHouse3
{

    /// <summary>
    /// Controls the collision detection and response behavior of a tile.
    /// </summary>
    public enum TileCollision
    {
        /// <summary>
        /// A passable tile is one which does not hinder player motion at all.
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A platform tile is one which behaves like a passable tile except when the
        /// player is above it. A player can jump up through a platform as well as move
        /// past it to the left and right, but can not fall down through the top of it.
        /// </summary>
        Platform = 2,
    }

    
    public class Tile : Circle
    {
		public TileCollision Collision { get; private set; }

        public Tile(Vector2 position, int width, int height, float speed, float jump, Color color, Texture2D texture = null, TileCollision collision = TileCollision.Passable)
        : base(position, width, speed, jump, color, texture)
        {
            this.Collision = collision;
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
