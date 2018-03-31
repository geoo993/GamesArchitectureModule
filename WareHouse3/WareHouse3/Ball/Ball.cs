using System;
using System.Linq;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

// https://www.gamedev.net/forums/topic/610640-xnac-method-for-jumping/
// https://stackoverflow.com/questions/20705928/how-can-i-double-jump-in-my-platform-game-code
namespace WareHouse3
{

    public class Ball: Circle
    {
    
        private Level Level { get; set; }
   
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
        private Rectangle PreviousBounds;
        
        private bool IsIntersecting = false;
        private bool OnCollisionEnter;
        private Tile PreviousTileCollided = null;
        
        /// <summary>
        /// colliding obstacles
        /// </summary>
        public List<Rectangle> Obstacles;
        
        public Ball(String name, Level level, Vector2 position, int radius, float speed, float jump, float mass, Color color, SoundEffect note, Texture2D texture = null, TileCollision collision = TileCollision.Passable)
        : base(name, position, radius, speed, jump, mass, color, note, texture, collision)
        {
            this.EnableParticles = true;
            this.HasJumped = true;
            this.particles = new List<Circle>();
            this.Obstacles = new List<Rectangle>();
            this.AvailableJumps = MaxJumps;
            this.Level = level;
            this.PreviousTileCollided = this;
            this.IsBorderEnabled = true;
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
       
            
        /// <summary>
        /// Detects and resolves all collisions between the player and his neighboring
        /// tiles. When a collision is detected, the player is pushed away along one
        /// axis to prevent overlapping. There is some special logic for the Y axis to
        /// handle platforms which behave differently depending on direction of movement.
        /// </summary>
        public void UpdateCollisions(List<Tile> tiles, Vector2 mapSize)
        {

            var tile = Level.ClosestTile(tiles, Position);
           
			TileCollision collision = tile.Collision;
			Rectangle tileBounds = tile.BoundingRectangle;
            
            if (Intersects(tileBounds)) {
                IsIntersecting = true;
                
                
                if (OnCollisionEnter == false && PreviousTileCollided.Name != tile.Name) {
                    OnCollisionEnter = true;
                }
                
                if (OnCollisionEnter && PreviousTileCollided.Name == tile.Name) {
                    OnCollisionEnter = false;
                }
                
				PreviousTileCollided = tile;
                
                if (OnCollisionEnter && tile.Note != null) {
                    tile.Note.Play();
                }
            
            } else {
                this.Ground = mapSize.Y;
                this.Ceiling = 0.0f;
                IsIntersecting = false;
                OnCollisionEnter = false;
            }
                        
            if (collision != TileCollision.Passable)
            {
               
                bool horizontalBoundary = (PreviousBounds.Center.X > tileBounds.Left && PreviousBounds.Center.X < tileBounds.Right);
				// If we crossed the top of a tile, we are on the ground.
				// If we crossed the bottom of a tile, we are hitting the tile from bellow.
				if (PreviousBounds.Bottom <= tileBounds.Top) {// && horizontalBoundary) {
					this.Ground = tileBounds.Top + 1;
				} else if (PreviousBounds.Top >= tileBounds.Bottom) {// && horizontalBoundary) {
					this.Ceiling = tileBounds.Bottom - 1;
				} else if (horizontalBoundary == false && IsIntersecting == false) {
                    this.Ground = mapSize.Y;
                    this.Ceiling = 0.0f;
                }
            }
            
            // Save the new bounds.
            PreviousBounds = BoundingRectangle;
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
                
				Acceleration.Y = (MotionState.mode == MotionState.Mode.falling) ? JumpSpeed : (JumpSpeed * JumpScale); // single and double jump speed
                HasJumped = true;
                DoJump = false;
            }
            
            if (HasJumped) {

                if (EnableParticles)
                {
                    //AddParticle(Position);
                }
                
                Acceleration.Y -= Gravity.Y;
                
                Velocity.Y -= (this.MotionState.IsFalling) ? Acceleration.Y * (1.0f / Mass) : Acceleration.Y;
                
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
        
            Circle particle = new Circle("",position, Radius, 0.0f, 0.0f, 1.0f, col, null);
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
        
        
        public override void UpdatePosition(GameTime gameTime, Vector2 mapSize)
        {
            
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            System.Diagnostics.Debug.Print("Elapsed Time "+ gameTime.ElapsedGameTime);
            
            this.Position += this.Velocity * elapsed;

            UpdateCollisions(Level.Tiles, mapSize);
            
            BallMovement(Ground);
            
            base.UpdatePosition(gameTime, mapSize);
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
