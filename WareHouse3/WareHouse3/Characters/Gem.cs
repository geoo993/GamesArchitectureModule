#region File Description
//-----------------------------------------------------------------------------
// Gem.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using TexturePackerLoader;

namespace WareHouse3
{
    /// <summary>
    /// A valuable item the player can collect.
    /// </summary>
    class Gem
    {
        private SpriteFrame spriteFrame;
        //private Vector2 origin;
        //private SoundEffect collectedSound;

        public const int PointValue = 30;

        // The gem is animated from a base position along the Y axis.
        private Vector2 basePosition;
        private float bounce;

        public Level Level
        {
            get { return level; }
        }
        Level level;

        /// <summary>
        /// Gets the current position of this gem in world space.
        /// </summary>
        public Vector2 Position
        {
            get
            {
                return basePosition + new Vector2(0.0f, bounce);
            }
        }

        /// <summary>
        /// Gets a circle which bounds this gem in world space.
        /// </summary>
        public Circle BoundingCircle
        {
            get
            {
                return new Circle(Position, TileInfo.Width / 3.0f);
            }
        }

        /// <summary>
        /// Constructs a new gem.
        /// </summary>
        public Gem(Level level, SpriteFrame spriteFrame, Vector2 position)
        {
            this.level = level;
            this.basePosition = position;
            this.spriteFrame = spriteFrame;
            this.spriteFrame.OriginType = SpriteFrame.FrameOrigin.center;
            LoadContent();
        }

        /// <summary>
        /// Loads the gem texture and collected sound.
        /// </summary>
        public void LoadContent()
        {
            //texture = Level.Content.Load<Texture2D>("Sprites/Gem");
            //origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            //collectedSound = Level.Content.Load<SoundEffect>("Sounds/GemCollected");
        }

        /// <summary>
        /// Bounces up and down in the air to entice players to collect them.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // Bounce control constants
            float BounceHeight = GemInfo.BounceHeight;//0.18f;
            float BounceRate = GemInfo.BounceRate;//3.0f;
            const float BounceSync = -0.75f;

            // Bounce along a sine curve over time.
            // Include the X coordinate so that neighboring gems bounce in a nice wave pattern.            
            double t = gameTime.TotalGameTime.TotalSeconds * BounceRate + Position.X * BounceSync;
            bounce = (float)Math.Sin(t) * BounceHeight * spriteFrame.Rectangle.Height;
        }

        /// <summary>
        /// Called when this gem has been collected by a player and removed from the level.
        /// </summary>
        /// <param name="collectedBy">
        /// The player who collected this gem. Although currently not used, this parameter would be
        /// useful for creating special powerup gems. For example, a gem could make the player invincible.
        /// </param>
        public void OnCollected(Player collectedBy)
        {
            //collectedSound.Play();
        }

        /// <summary>
        /// Draws a gem in the appropriate color.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteRender spriteRender)
        {
            spriteRender.Draw( spriteFrame, Position, GemInfo.Color, 0.0f, 1.0f, SpriteEffects.None);
                
        }
    }
}
