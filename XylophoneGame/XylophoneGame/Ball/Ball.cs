using System;
using System.Diagnostics;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

// https://www.gamedev.net/forums/topic/610640-xnac-method-for-jumping/
// https://stackoverflow.com/questions/20705928/how-can-i-double-jump-in-my-platform-game-code
namespace XylophoneGame
{

    public class Ball: Circle
    {
        /// <summary>
        /// trail particle of the ball
        /// </summary>
        private Particles Particles;
        
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

        public bool IsIntersectingNote { get; private set; }
        public bool IsIntersectingTimeItem { get; private set; }
        public bool IsIntersectingMusicNoteItem { get; private set; }
        
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
            this.Particles = new Particles(color);
            this.AvailableJumps = MaxJumps;
            this.PreviousNoteCollided = null;
            this.IsBorderEnabled = true;
            this.IsIntersectingNote = false;
            this.IsIntersectingTimeItem = false;
            this.IsIntersectingMusicNoteItem = false;
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


        private void UpdateCollisions(Note note, TimeItem timeItem, MusicNoteItem musicNoteItem, Vector2 mapSize)
        {
            PreviousNote = PreviousNoteCollided;
            var noteBounds = note.BoundingRectangle;
            var previousTileName = (PreviousNoteCollided != null) ? PreviousNoteCollided.Name : "";

            if (Intersects(noteBounds))
            {
                IsIntersectingNote = true;
                OnCollisionExit = false;

                if (OnCollisionEnter == false && (previousTileName != note.Name || PreviousNoteCollided == null))
                {
                    OnCollisionEnter = !MotionState.state.IsJumping;
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
                IsIntersectingNote = false;
                OnCollisionEnter = false;
                OnCollisionExit = (PreviousNoteCollided != null);
                PreviousNoteCollided = null;
            }

			IsIntersectingTimeItem = false;
            if (timeItem != null)
            {
                var timerItemBounds = timeItem.BoundingRectangle;
                if (IntersectsCircle(timerItemBounds) && timeItem.IsEnabled)
                {
                    IsIntersectingTimeItem = true;
                }
            }

			IsIntersectingMusicNoteItem = false;
            if (musicNoteItem != null) { 
                var musicNoteItemBounds = musicNoteItem.BoundingRectangle;
                if (IntersectsCircle(musicNoteItemBounds) && musicNoteItem.IsEnabled)
                {
                    IsIntersectingMusicNoteItem = true;
                }
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
				} else if (horizontalBoundary == false && IsIntersectingNote == false) {
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
                    Particles.AddTrailParticle(Position, Radius);
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
            
            Particles.UpdateTrailParticles();
        }
       
        public override void UpdatePosition(GameTime gameTime, Vector2 mapSize)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            this.Position += this.Velocity * elapsed;
        }
        
        public void Update(GameTime gameTime, Vector2 mapSize, ref string noteToSelect, ref bool isNoteMatched, ref bool isNoteError, ref Note closestNote, ref TimeItem closestTimeItem, ref MusicNoteItem closestMusicNoteItem) {
			
			this.UpdateCollisions(closestNote, closestTimeItem, closestMusicNoteItem, mapSize);
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
                
                closestNote.Pressed(this);
            }
			
			if (OnCollisionExit)
			{
                PreviousNote.Released();
			}
            
			base.UpdatePosition(gameTime, mapSize);
        }
        
        public override bool Intersects(Rectangle rectangle)
        {
            return base.Intersects(rectangle);
        }
        
        public override void Draw(SpriteBatch spriteBatch, Rectangle screenSafeArea) 
        {
            
            Particles.DrawTrailParticles(spriteBatch, screenSafeArea);
            base.Draw(spriteBatch, screenSafeArea);
        }

        public override void Destroy()
        {
            Particles.Destroy();
            Particles = null;

            if (PreviousNoteCollided != null)
            {
                PreviousNoteCollided.Destroy();
                PreviousNoteCollided = null;
            }

            if (PreviousNote != null)
            {
                PreviousNote.Destroy();
                PreviousNote = null;
            }
            
            base.Destroy();
        }
        
    }
}
