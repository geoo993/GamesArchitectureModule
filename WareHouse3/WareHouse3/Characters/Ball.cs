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
        
        public Ball(Vector2 position, int radius, float speed, float jump, Color color, Texture2D texture = null, TileCollision collision = TileCollision.Passable)
        : base(position, radius, speed, jump, color, texture, collision)
        {
            this.EnableParticles = true;
            this.HasJumped = true;
            this.particles = new List<Circle>();
            this.Obstacles = new List<Rectangle>();
            this.AvailableJumps = MaxJumps;
        }
        
        
        public void UpdateBoundaries(Tile tile, Vector2 mapSize) {
            
            this.LeftBoundary = (tile.BoundingRectangle.Right <= this.Position.X) ? tile.BoundingRectangle.Right : 0.0f;
            this.RightBoundary = (tile.BoundingRectangle.Left > this.Position.X) ? tile.BoundingRectangle.Left : mapSize.X;
            bool horizontalBoundary = (this.Position.X > tile.BoundingRectangle.Left &&
                                       this.Position.X < tile.BoundingRectangle.Right);
            
            this.Ground = (this.BoundingRectangle.Bottom <= tile.BoundingRectangle.Top && horizontalBoundary) ? tile.BoundingRectangle.Top : this.Ground = mapSize.Y;
            this.Ceiling = (this.BoundingRectangle.Top >= tile.BoundingRectangle.Bottom && horizontalBoundary) ? tile.BoundingRectangle.Bottom : 0.0f;
        }

        public void IsLeft(ButtonAction buttonState) {
            if (buttonState == ButtonAction.DOWN)
            {
                Velocity.X = -MoveSpeed.X;
                LeftPressed = true;
            }
            
            if (buttonState == ButtonAction.UP)
            {
                LeftPressed = false;
            } 
        }
        
        public void IsRight(ButtonAction buttonState) {
             if (buttonState == ButtonAction.DOWN)
            {
                Velocity.X = MoveSpeed.X;
                RightPressed = true;
            } 
            
            if (buttonState == ButtonAction.UP)
            {
                RightPressed = false;
            } 
        }
        
        public void IsJump() {
            DoJump = true;
        }
        
        public void IsScaledUp() {
            Scale += 0.01f;
        }
        
        public void IsScaledDown() {
            Scale -= 0.01f;
        }
        
        public void UpdateCollisions(List<Tile> tiles, Vector2 mapSize)
        {

            var tile = ClosestObstacle(tiles);
			UpdateBoundaries(tile, mapSize);

            
            if (this.Intersects(tile.BoundingRectangle))
            {
                
                //System.Diagnostics.Debug.Print("Colliding");
				tile.Color = Color.White;
            }
            else
            {
                //System.Diagnostics.Debug.Print("Not Colliding");
                tile.Color = tile.InitialColor;
                this.LeftBoundary = 0.0f;
                this.RightBoundary = mapSize.X;
            }
            
        }

        public override void UpdatePosition(GameTime gameTime, Vector2 mapSize)
        {

            this.Position += this.Velocity;
            
            BallMovement(Ground);
            
            base.UpdatePosition(gameTime, mapSize);
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
                    //AddParticle(Position);
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
            
			//UpdateParticles();
            
        }
       
        private void AddParticle(Vector2 position)
        {
            byte red = (byte)GameInfo.Random.Next(0, 255);
            byte green = (byte)GameInfo.Random.Next(0, 255);
            byte blue = (byte)GameInfo.Random.Next(0, 255);
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
        
        
        public Tile ClosestObstacle(List<Tile> tiles)
        {
            // https://stackoverflow.com/questions/33145365/what-is-the-most-effective-way-to-get-closest-target
            return tiles
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
