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

namespace XylophoneGame
{
        /// <summary>
    /// Controls playback of an Animation.
    /// </summary>
    struct AnimationPlayer
    {
        /// <summary>
        /// Gets the animation which is currently playing.
        /// </summary>
        public Animation Animation { get; private set; }
        
        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void PlayAnimation(Animation animation)
        {
            // If this animation is already running, do not restart it.
            if (Animation == animation)
                return;

            // Start the new animation.
            this.Animation = animation;
            this.Animation.FrameIndex = 0;
            this.Animation.ElapsedTime = 0.0f;
        }

        /// <summary>
        /// Advances the time position and draws the current frame of the animation.
        /// </summary>
        public void Update(GameTime gameTime, Vector2 position)
        {
            if (Animation == null)
                return;
                
            // Do not update the game if we are not active
			if (Animation.Active == false)
				return;
          
            // Update the elapsed time
            Animation.ElapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            // Update position
            Animation.Position = position;
        }
        
        public void Draw(SpriteBatch spriteBatch) {
            
            if (Animation == null)
                return;
            
            // Process passing time.
            while (Animation.ElapsedTime > Animation.FrameTime)
            {
                Animation.ElapsedTime -= Animation.FrameTime;

                // Advance the frame index; looping or clamping as appropriate.
                if (Animation.IsLooping)
                {
                    Animation.FrameIndex = (Animation.FrameIndex + 1) % Animation.FrameCount;
                    
                    // If we are not looping deactivate the animation
                    //if (Animation.IsLooping == false)
                        //Animation.Active = false;
                }
                else
                {
                    Animation.FrameIndex = Math.Min(Animation.FrameIndex + 1, Animation.FrameCount - 1);
                }
            }

             // Only draw the animation when we are active
            if (Animation.Active)
            {
                spriteBatch.Draw(Animation.Texture, Animation.DestinationRect, Animation.SourceRect, Animation.Color, 0.0f, Animation.Origin, Animation.Effects, 0.0f);
            }
        }
        
    }
    
}
