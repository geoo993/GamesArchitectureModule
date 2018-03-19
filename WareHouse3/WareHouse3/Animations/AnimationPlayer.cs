#region File Description
//-----------------------------------------------------------------------------
// AnimationPlayer.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TexturePackerLoader
{
    /// <summary>
    /// Controls playback of an Animation.
    /// </summary>
    public class AnimationPlayer
    {
    
        private readonly SpriteSheet spriteSheet;
        
        /// <summary>
        /// Gets the current frame of the currently playing sprite.
        /// </summary>
        public SpriteFrame CurrentSprite
        {
            get { return currentSprite; }
        }
        SpriteFrame currentSprite;
        
        /// <summary>
        /// Gets the animation which is currently playing.
        /// </summary>
        public Animation Animation
        {
            get { return animation; }
        }
        Animation animation;
        
        
        /// <summary>
        /// Gets the index of the current frame in the animation.
        /// </summary>
        public int FrameIndex
        {
            get { return frameIndex; }
        }
        private int frameIndex;

        /// <summary>
        /// The amount of time in seconds that the current frame has been shown for.
        /// </summary>
        private float time;
        
        /// <summary>
        /// The current position of at player.
        /// </summary>
        public Vector2 CurrentPosition 
        { 
            get { return this.currentPosition; } 
        }
        private Vector2 currentPosition;
        
        public AnimationPlayer(SpriteSheet spriteSheet, Vector2 initialPosition, Animation animation)
        {
            this.spriteSheet = spriteSheet;
			this.currentPosition = initialPosition;
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
            this.currentSprite = spriteSheet.Sprite(animation.Sprites[frameIndex]);
            this.currentSprite.OriginType = SpriteFrame.FrameOrigin.bottomcenter;
            
		}
        
        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void PlayAnimation(Animation animation)
        {
            
            // If this animation is already running, do not restart it.
            if (Animation == animation)
                return;

            // Start the new animation.
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
        }

        /// <summary>
        /// Advances the time position and draws the current frame of the animation.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteRender spriteRender, Vector2 position, SpriteEffects spriteEffects)
        {
            if (Animation == null)
                throw new NotSupportedException("No animation is currently playing.");

            // Process passing time.
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (time > Animation.FrameTime)
            {
                time -= Animation.FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (Animation.IsLooping)
                {
                    frameIndex = (frameIndex + 1) % Animation.FrameCount;
                }
                else
                {
                    frameIndex = Math.Min(frameIndex + 1, Animation.FrameCount - 1);
                }
            }

            currentSprite = this.spriteSheet.Sprite(animation.Sprites[frameIndex]);
            currentPosition = position;
            
            // Draw character on screen
            spriteRender.Draw(
                currentSprite, 
                CurrentPosition, 
                Color.White, 
                0.0f, 
                1.0f,
                spriteEffects);
          
        }
    }
}
