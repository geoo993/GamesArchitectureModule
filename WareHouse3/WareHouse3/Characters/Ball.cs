using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace WareHouse3
{
    public class Ball: Circle
    {
        /// <summary>
        /// trail particle of the ball
        /// </summary>
        List<Circle> particles;
        
        /// <summary>
        /// should the trail particle be enabled
        /// </summary>
        public bool EnableParticles;
        
        /// <summary>
        /// the speed in which the ball falls down
        /// </summary>
        readonly Vector2 Gravity = new Vector2(0, 0.18f);
        
        /// <summary>
        /// the movement speed the ball
        /// </summary>
        private Vector2 Velocity;
        
        /// <summary>
        /// find out if the ball is in the air
        /// </summary>
        public bool HasJumped { get; private set; }
        private bool JumpCountState = false;
        private int JumpingCount = 0;
        
        /// <summary>
        /// is the left and right keyboard buttons pressed
        /// </summary>
        private bool LeftPressed, RightPressed;
        
        public Ball(Vector2 position, int radius, float speed, float jump, Color color, Texture2D texture = null)
        : base(position, radius, speed, jump, color, texture)
        {
            this.HasJumped = true;
            this.particles = new List<Circle>();
        }
        
        protected override void LeftMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                Velocity.X = -MoveSpeed;
                LeftPressed = true;
            }
            
            if (buttonState == ButtonAction.UP)
            {
                LeftPressed = false;
            } 
        }
        
        protected override void RightMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                Velocity.X = MoveSpeed;
                RightPressed = true;
            } 
            
            if (buttonState == ButtonAction.UP)
            {
				RightPressed = false;
            } 
        }
        
        protected override void UpMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                Scale += 0.01f;
            }
        }
        
        
        protected override void DownMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                Scale -= 0.01f;
            }
        }
   
        protected override void JumpMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN && HasJumped == false)
            {
                Position.Y -= JumpSpeed;
                Velocity.Y = -(JumpSpeed / 2);
                HasJumped = true;
                
                
                /*
                if(JumpingCount > 0)
                {
                    //ySpeed = speedMax;
                    JumpingCount = 0;
                    JumpCountState = true;
                    HasJumped = true;
                }
                if(HasJumped == false)
                {
                    //ySpeed = speedMax;
                    HasJumped = true;
                }
                */
                
            }
        }
        
        public void PlayerMovement()
        {
            //// left of screen
            //if(myShape.x-(lenggth*2)< 0)
            //{   
            //    leftSpeed = 0;  
            //    leftForce = 0;
            //}
			//if(left == true)
			//{
			//	rightSpeed = 4;  
			//	rightForce = 1.5;
			//	myShape.x -=  leftSpeed * leftSpeed;
			//} 
            
            //// right of screen
            //if(myShape.x+(lenggth*2)> stage.width)
            //{
            //    rightSpeed = 0;  
            //    rightForce = 0;
            //}
            //if(right == true)
            //{   
            //    leftSpeed = 3;
            //    leftForce = 1.5; 
            //    myShape.x +=  rightSpeed * rightForce;
            //}
            
            if(HasJumped == true)
            {
                if(JumpCountState == false)
                {
                    JumpingCount++;
                }
                
                //ySpeed -= gravity;
                //myShape.y -= ySpeed;
                
            }
            if(HasJumped == false)
            {
                JumpingCount = 0;
                JumpCountState = false;
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

            if (BoundingRectangle.Bottom >= screenSize.Y + Origin.Y) {
                HasJumped = false;
            } else {
                HasJumped = true;
            }

            if (HasJumped == false) {
                Velocity.Y = 0.0f;
            }
            
            UpdateParticles();
            
            base.UpdatePosition(gameTime, screenSize);
            
        }
       
        private void AddParticle(Vector2 position)
        {
            Circle particle = new Circle(position, Radius, 0.0f, 0.0f, ColorExtension.Random);
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
