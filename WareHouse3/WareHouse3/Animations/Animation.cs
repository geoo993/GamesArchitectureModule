namespace TexturePackerLoader
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Animation
    {
    
        /// <summary>
        /// Measured in pixels per second
        /// </summary>
        public Vector2 CharacterVelocity { get; private set; }

        public SpriteEffects Effect { get; private set; }

        /// <summary>
        /// Number of sprite for animation.
        /// </summary>
        public string[] Sprites { get; private set; }
   
        /// <summary>
        /// Duration of time to show each frame.
        /// </summary>
		public TimeSpan TimePerFrame { get; private set; }
        public float FrameTime
        {
            get { return frameTime; }
        }
        float frameTime;
        
        /// <summary>
        /// When the end of the animation is reached, should it
        /// continue playing from the beginning?
        /// </summary>
        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;

        /// <summary>
        /// Gets the number of frames in the animation.
        /// </summary>
        public int FrameCount
        {
            get { return Sprites.Length; }
        }
      
        

    }
}
