using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// https://www.youtube.com/watch?v=ZLxIShw-7ac

namespace WareHouse3
{
    public class Character: Box
    {
        /// <summary>
        /// trail particle of the shape
        /// </summary>
        List<Box> particles;
        
        /// <summary>
        /// should the trail particle be enabled
        /// </summary>
        public bool EnableParticles;
        
        /// <summary>
        /// the speed in which the object falls down
        /// </summary>
        readonly Vector2 Gravity = new Vector2(0, 0.18f);
        
        /// <summary>
        /// the movement speed the object
        /// </summary>
        private Vector2 Velocity;
        
        /// <summary>
        /// find out if the object is in the air
        /// </summary>
        public bool HasJumped { get; private set; }
        
        /// <summary>
        /// is the left and right keyboard buttons pressed
        /// </summary>
        private bool LeftPressed, RightPressed;
        
        public Character(Vector2 position, int width, int height, float speed, float jump, Color color, Texture2D texture = null)
        : base(position, width, height, speed, jump, color, texture)
        {
            this.HasJumped = true;
            this.particles = new List<Box>();
        }
        
        protected override void LeftMovement(ButtonAction buttonState, Vector2 amount)
        {
            LeftPressed = false;
            if (buttonState == ButtonAction.DOWN)
            {
                Velocity.X = -MoveSpeed;
                LeftPressed = true;
            }
        }
        
        protected override void RightMovement(ButtonAction buttonState, Vector2 amount)
        {
            RightPressed = false;
            if (buttonState == ButtonAction.DOWN)
            {
                Velocity.X = MoveSpeed;
                RightPressed = true;
            } 
        }
       
        protected override void UpMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                Scale += 0.02f;
            }
        }
        
        
        protected override void DownMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                Scale -= 0.02f;
            }
        }
        
        protected override void JumpMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN && HasJumped == false)
            {
                Position.Y -= JumpSpeed;
                Velocity.Y = -(JumpSpeed / 2);
                HasJumped = true;
            }
        }
        
        public override void UpdatePosition(GameTime gameTime, Vector2 screenSize)
        {
    
            this.Position += this.Velocity;
            
            if (LeftPressed == false && RightPressed == false) {
                Velocity.X = 0.0f;
            }
            
            if (HasJumped) {
                float i = 1;
                Velocity.Y += Gravity.Y * i;

                if (EnableParticles)
                {
                    AddParticle(Position);
                }
            }
            
            HasJumped = !(BoundingRectangle.Bottom >= screenSize.Y + Origin.Y);

            if (HasJumped == false) {
                Velocity.Y = 0.0f;
            }
            
            UpdateParticles();
            
			base.UpdatePosition(gameTime, screenSize);
            
        }
       
        private void AddParticle(Vector2 position)
        {
            Box particle = new Box(position, Width, Height, 0.0f, 0.0f, ColorExtension.Random);
            particles.Add(particle);
        }
        
        private void UpdateParticles() {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Opacity -= 0.02f;
                particles[i].Scale -= 0.02f;
                if (particles[i].Opacity <= 0.0f || particles[i].Scale <= 0.001f ) {
                    particles.RemoveAt(i);
                }
            }
        }

        public override bool Intersects(Rectangle rectangle)
        {
            return base.Intersects(rectangle);
        }
        
        public override void Render(SpriteBatch spriteBatch) 
        {

            // Draw the particles
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].Render(spriteBatch);
            }
            
			base.Render(spriteBatch);
        }
        
    }
}
