using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace XylophoneGame
{
    public class Animation
    {
        
        /// <summary>
		// The image representing the collection of images used for animation
        /// All frames in the animation arranged horizontally.
        /// </summary>
        public Texture2D Texture { get; private set; }
        
        
        /// <summary>
        /// Gets the width of the frame.
        /// Gets the width of a frame in the animation.
        /// </summary>
        /// <value>The width of the frame.</value>
        //public int FrameWidth { get; private set; }
        public int FrameWidth {  get { return Texture.Width / FrameCount; } }
        
        /// <summary>
        /// Gets the height of the frame.
        /// Gets the height of a frame in the animation.
        /// </summary>
        /// <value>The height of the frame.</value>
        public int FrameHeight {  get { return Texture.Height; } }
        
        /// <summary>
		// The time since we last updated the frame
		///// The amount of time in seconds that the current frame has been shown for.
        /// </summary>
        public float ElapsedTime { get; set; }
        
        /// <summary>
		// The time we display a frame until the next one
		/// Duration of time to show each frame.
		/// </summary>
        public int FrameTime { get; private set; }
        
        /// <summary>
        // Gets the number of frames in the animation.
		// The number of frames that the animation contains.
        /// </summary>
        public int FrameCount { get; private set; }
        
        /// <summary>
		// The index of the current frame we are displaying
        /// Gets the index of the current frame in the animation.
        /// </summary>
        public int FrameIndex { get; set; }

        // The color of the frame we will be displaying
        public Color Color { get; private set; }
        
        /// <summary>
		// The area of the image strip we want to display
        /// Grab the correct frame in the image texture by multiplying the currentFrame index by the frame width
        /// </summary>
        /// <value>The source rect.</value>
        public Rectangle SourceRect {
            get {
                return new Rectangle(FrameIndex * FrameWidth, 0, FrameWidth, FrameHeight);
            
            }
        }
        
        /// <summary>
		/// The area where we want to display the image texture in the game
		// Grab the correct frame in the image texture by multiplying the currentFrame index by the frame width and also including the scale
        /// </summary>
        /// <value>The destination rect.</value>
        public Rectangle DestinationRect
        {
            get
            {
                return new Rectangle((int)Position.X - (int)(FrameWidth * Scale) / 2,
                                     (int)Position.Y - (int)(FrameHeight * Scale) / 2,
                                     (int)(FrameWidth * Scale),
                                     (int)(FrameHeight * Scale));
            }
        }
        
        // <summary>
        /// Gets a texture origin at the bottom center of each frame.
        /// </summary>
        protected FrameOrigin.OriginType OriginType;
        public Vector2 Origin
        {
            get { 
                return FrameOrigin.GetOrigin(FrameWidth, FrameHeight, OriginType); // //new Vector2(FrameWidth / 2.0f, FrameHeight / 2.0f); } 
            }
        }
        
        // The state of the Animation
        public bool Active;

        /// <summary>
        // Determines if the animation will keep playing or deactivate after one run
        /// When the end of the animation is reached, should it
        /// continue playing from the beginning?
        /// </summary>
		public bool IsLooping;

        // Width of a given frame
        public Vector2 Position;
        
        /// <summary>
        // The scale used to display the sprite strip
        /// <summary>
        public float Scale;
        
        /// <summary>
        // The scale used to display the sprite strip
        /// <summary>
        public SpriteEffects Effects { get; private set; }
        
        /// <summary>
        /// Constructors a new animation.
        /// </summary>        
        public Animation(Texture2D texture, Vector2 position, float scale, int frameCount, int frametime, Color color, bool looping)
        {
            // Keep a local copy of the values passed in
            this.Color = color;
            this.Scale = scale;
            this.IsLooping = looping;
            this.Position = position;
            this.Texture = texture;
			this.FrameTime = frametime;
            this.FrameCount = frameCount;
            this.OriginType = FrameOrigin.OriginType.topLeft;
            
            // Set the time to zero
            this.ElapsedTime = 0;
            this.FrameIndex = 0;

            // Set the Animation to active by default
            this.Active = true;
            this.Effects = SpriteEffects.None;
        }
        
        
    }
    
    
}
