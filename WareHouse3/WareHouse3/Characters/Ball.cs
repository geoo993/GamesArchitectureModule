using System;
using System.Linq;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// https://www.gamedev.net/forums/topic/610640-xnac-method-for-jumping/
// https://stackoverflow.com/questions/20705928/how-can-i-double-jump-in-my-platform-game-code
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
        readonly Vector2 Gravity = new Vector2(0, 0.45f);
        
        /// <summary>
        /// the movement speed the ball
        /// </summary>
        private Vector2 Velocity;
        private Vector2 Acceleration;
        
        /// <summary>
        /// find out if the ball is in the air
        /// </summary>
        private bool HasJumped { get; set; }
        private bool DoJump { get; set; }
        private bool CanJump { get; set; }
        private int AvailableJumps;
        private readonly int MaxJumps = 2;
        
        /// <summary>
        /// is the left and right keyboard buttons pressed
        /// </summary>
        private bool LeftPressed, RightPressed;

        /// <summary>
        /// colliding obstacles
        /// </summary>
        public List<Rectangle> Obstacles;
        
        public Ball(Vector2 position, int radius, float speed, float jump, Color color, Texture2D texture = null)
        : base(position, radius, speed, jump, color, texture)
        {
            this.EnableParticles = true;
            this.HasJumped = true;
            this.particles = new List<Circle>();
            this.Obstacles = new List<Rectangle>();
            this.AvailableJumps = MaxJumps;
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
            if (buttonState == ButtonAction.PRESSED)
            {
				DoJump = true;
            }
        }
        
        public void UpdateBoundaries(Character obstacle, Vector2 screenSize) {
            
            this.LeftBoundary = (obstacle.BoundingRectangle.Right <= this.Position.X) ? obstacle.BoundingRectangle.Right : 0.0f;
            this.RightBoundary = (obstacle.BoundingRectangle.Left > this.Position.X) ? obstacle.BoundingRectangle.Left : screenSize.X;
            bool horizontalBoundary = (this.Position.X > obstacle.BoundingRectangle.Left &&
                                       this.Position.X < obstacle.BoundingRectangle.Right);
            
            this.Ground = (this.BoundingRectangle.Bottom <= obstacle.BoundingRectangle.Top && horizontalBoundary) ? obstacle.BoundingRectangle.Top : this.Ground = screenSize.Y;
            this.Ceiling = (this.BoundingRectangle.Top >= obstacle.BoundingRectangle.Bottom && horizontalBoundary) ? obstacle.BoundingRectangle.Bottom : 0.0f;
        }

        public void UpdateCollisions(List<Character> obstacles, Vector2 screenSize)
        {

            var obstacle = ClosestObstacle(obstacles);
			UpdateBoundaries(obstacle, screenSize);
            
            if (this.Intersects(obstacle.BoundingRectangle))
            {
                
                //System.Diagnostics.Debug.Print("Colliding");
				obstacle.Color = Color.White;
            }
            else
            {
                //System.Diagnostics.Debug.Print("Not Colliding");
                obstacle.Color = obstacle.InitialColor;
                this.LeftBoundary = 0.0f;
                this.RightBoundary = screenSize.X;
            }
            
        }

        public override void UpdatePosition(GameTime gameTime, Vector2 screenSize)
        {

            this.Position += this.Velocity;

            BallMovement(Ground);
            
            base.UpdatePosition(gameTime, screenSize);
        }
        
        
        private void BallMovement(float ground) {
        
            if (LeftPressed == false && RightPressed == false) {
                Velocity.X = 0.0f;
            }

            if (DoJump && CanJump)
            {

                float JumpScale = (float)AvailableJumps / (float)MaxJumps;
                
                // Can you jump again??
                AvailableJumps--;

                if (AvailableJumps <= 0)
                {
                    CanJump = false;
                }
                
				Acceleration.Y = (MotionState.mode == MotionState.Mode.falling) ? JumpSpeed : (JumpSpeed * JumpScale);
                HasJumped = true;
                DoJump = false;
            }
            
            if (HasJumped) {

                if (EnableParticles)
                {
                    AddParticle(Position);
                }
                
                this.Acceleration.Y -= Gravity.Y;
                this.Velocity.Y -= Acceleration.Y;
            }


            if (HasJumped == false) {
                Velocity.Y = 0.0f;
            }
            
            if (BoundingRectangle.Bottom >= ground) {
                if (Velocity.Y >= 0.0f) {
                    Acceleration.Y = 0.0f;
                    AvailableJumps = MaxJumps;
                    CanJump = true;
                }
                HasJumped = false;
            } else {
                HasJumped = true;
            }
            
			UpdateParticles();
            
        }
       
        private void AddParticle(Vector2 position)
        {
            Random r = new Random(DateTime.Now.Millisecond); 
            byte red = (byte)r.Next(0, 255);
            byte green = (byte)r.Next(0, 255);
            byte blue = (byte)r.Next(0, 255);
            Color col =  new Color(red, green, blue);
        
            Circle particle = new Circle(position, Radius, 0.0f, 0.0f, col);
            particles.Add(particle);
        }
        
        private void UpdateParticles() {

            if (particles.Count > 0) {
            
                for (int i = 0; i < particles.Count; i++)
                {
                    particles[i].Opacity -= 0.02f;
                    particles[i].Scale -= 0.02f;
                    if (particles[i].Opacity <= 0.0f || particles[i].Scale <= 0.001f)
                    {
                        particles.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }
        }
        
        
        public Character ClosestObstacle(List<Character> obstacles)
        {
            // https://stackoverflow.com/questions/33145365/what-is-the-most-effective-way-to-get-closest-target
            return obstacles
                .OrderBy(o => (o.Position - Position).LengthSquared())
                .FirstOrDefault();
        }
       

        public override bool Intersects(Rectangle rectangle)
        {
            return base.Intersects(rectangle);
        }
        
        public override void Render(SpriteBatch spriteBatch) 
        {

            // Draw the particles
            foreach (Circle particle in particles)
            {
                particle.Render(spriteBatch);
            }
            base.Render(spriteBatch);
        }
        
    }
}
