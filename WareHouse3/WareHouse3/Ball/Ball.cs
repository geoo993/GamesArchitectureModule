using System;
using System.Linq;
using System.Diagnostics;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

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
        private Rectangle PreviousBounds;

        private bool IsIntersecting;
        private bool OnCollisionEnter;
        private bool OnCollisionExit;
        private Note PreviousNoteCollided;
        public Note PreviousNote { get; private set; }
        public String NoteSelected { get; private set; }
        
        public Ball(String name, Level level, Vector2 position, int radius, float speed, float jump, float mass, Color color, SoundEffect note, Texture2D texture = null, TileCollision collision = TileCollision.Passable)
        : base(name, position, radius, speed, jump, mass, color, note, texture, collision)
        {
            this.EnableParticles = true;
            this.HasJumped = true;
            this.particles = new List<Circle>();
            this.AvailableJumps = MaxJumps;
            this.PreviousNoteCollided = null;
            this.IsBorderEnabled = true;
            this.IsIntersecting = false;
        }

        public void SetLeftMovement(ButtonAction buttonState)
        {
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
        
        public void SetRightMovement(ButtonAction buttonState)
        {
            
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
        
        public void SetUpMovement(ButtonAction buttonState)
        {
            if (buttonState == ButtonAction.DOWN)
            {
				Scale += 0.01f;
            }
        }
        
        
        public void SetDownMovement(ButtonAction buttonState)
        {
            if (buttonState == ButtonAction.DOWN)
            {
				Scale -= 0.01f;
            }
        }
   
        public void SetJumpMovement(ButtonAction buttonState)
        {
            if (buttonState == ButtonAction.PRESSED)
            {
                DoJump = true;
            }
        }


        private void UpdateCollisions(Note note, Vector2 mapSize)
        {
            PreviousNote = PreviousNoteCollided;
			var tileBounds = note.BoundingRectangle;
            var previousTileName = (PreviousNoteCollided != null) ? PreviousNoteCollided.Name : "";

            if (Intersects(tileBounds))
            {
                IsIntersecting = true;
                OnCollisionExit = false;

                if (OnCollisionEnter == false && (previousTileName != note.Name || PreviousNoteCollided == null))
                {
                    OnCollisionEnter = true;
                }

                if (OnCollisionExit == false && previousTileName != note.Name && PreviousNoteCollided != null)
                {
                    OnCollisionExit = true;
                }

                if (OnCollisionEnter && previousTileName == note.Name)
                {
                    OnCollisionEnter = false;
                }

                PreviousNoteCollided = note;
            }
            else
            {
                Ground = mapSize.Y;
                Ceiling = 0.0f;
                IsIntersecting = false;
                OnCollisionEnter = false;
                OnCollisionExit = (PreviousNoteCollided != null);
				PreviousNoteCollided = null;
            }
          
        }
        
	    public void UpdateBounds(Note note, Vector2 mapSize)
        {
            var tileBounds = note.BoundingRectangle;
            var collision = note.Collision;
            
            if (collision != TileCollision.Passable)
            {
               
                bool horizontalBoundary = (PreviousBounds.Center.X > tileBounds.Left && PreviousBounds.Center.X < tileBounds.Right);
				// If we crossed the top of a tile, we are on the ground.
				// If we crossed the bottom of a tile, we are hitting the tile from bellow.
				if (PreviousBounds.Bottom <= tileBounds.Top) {
					this.Ground = tileBounds.Top + 1;
				} else if (PreviousBounds.Top >= tileBounds.Bottom) {
					this.Ceiling = tileBounds.Bottom - 1;
				} else if (horizontalBoundary == false && IsIntersecting == false) {
                    this.Ground = mapSize.Y;
                    this.Ceiling = 0.0f;
                }
            }
            
            // Save the new bounds.
            PreviousBounds = BoundingRectangle;
        }

        
        private void UpdateMovement(float ground) {
        
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
                
				Acceleration.Y = (this.MotionState.state.IsFalling) ? JumpSpeed : (JumpSpeed * JumpScale); // single and double jump speed
                HasJumped = true;
                DoJump = false;
            }
            
            if (HasJumped) {

                if (EnableParticles)
                {
                    //AddParticle(Position);
                }
                
                Acceleration.Y -= Gravity.Y;
                
                Velocity.Y -= (this.MotionState.state.IsFalling) ? Acceleration.Y * (1.0f / Mass) : Acceleration.Y;
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
            
            this.Position += this.Velocity * elapsed;
        }
        
        public void Update(GameTime gameTime, Vector2 mapSize, ref string noteToSelect, ref bool isNoteMatched, ref bool isNoteError, ref Note closestNote) {
			
			this.UpdateCollisions(closestNote, mapSize);
			this.UpdateBounds(closestNote, mapSize);
			this.UpdateMovement(Ground);
            
			this.NoteSelected = null;

			if (OnCollisionEnter)
			{
				
				if (noteToSelect == closestNote.NoteName) {
					NoteSelected = closestNote.NoteName;
                    isNoteMatched = true;
				} else {
                    isNoteError = true;
                }
                
                if (closestNote.NoteSound != null) {
                    closestNote.NoteSound.Play();
                }
            }
			
			if (OnCollisionExit)
			{
				
			}
            
			base.UpdatePosition(gameTime, mapSize);
        }
        
        public override bool Intersects(Rectangle rectangle)
        {
            return base.Intersects(rectangle);
        }
        
        public override void Draw(SpriteBatch spriteBatch, Rectangle screenSafeArea) 
        {
            // Draw the particles
            foreach (Circle particle in particles)
            {
                particle.Draw(spriteBatch, screenSafeArea);
            }
            base.Draw(spriteBatch, screenSafeArea);
        }

        public override void Destroy()
        {
            foreach (Circle cir in particles){
                cir.Destroy();
            }
            particles.Clear();

            PreviousNoteCollided.Destroy();
            PreviousNoteCollided = null;

            PreviousNote.Destroy();
            PreviousNote = null;
            
            base.Destroy();
        }
        
    }
}
