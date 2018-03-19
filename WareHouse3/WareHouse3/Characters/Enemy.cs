#region File Description
//-----------------------------------------------------------------------------
// Enemy.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TexturePackerLoader;

namespace WareHouse3
{
    /// <summary>
    /// Facing direction along the X axis.
    /// </summary>
    enum FaceDirection
    {
        Left = -1,
        Right = 1,
    }

    /// <summary>
    /// A monster who is impeding the progress of our fearless adventurer.
    /// </summary>
    class Enemy
    {
        public Level Level
        {
            get { return level; }
        }
        Level level;

        /// <summary>
        /// Position in world space of the bottom center of this enemy.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }
        Vector2 position;

        private Rectangle localBounds;
        /// <summary>
        /// Gets a rectangle which bounds this enemy in world space.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                return sprite.CurrentSprite.Rectangle;
            }
        }
        
       
        // Animations
        private Animation runAnimation;
        private Animation idleAnimation;
        private AnimationPlayer sprite;

        /// <summary>
        /// The direction this enemy is facing and moving along the X axis.
        /// </summary>
        private FaceDirection direction = FaceDirection.Left;

        /// <summary>
        /// How long this enemy has been waiting before turning around.
        /// </summary>
        private float waitTime;

        /// <summary>
        /// How long to wait before turning around.
        /// </summary>
        private const float MaxWaitTime = 0.5f;

        /// <summary>
        /// The speed at which this enemy moves along the X axis.
        /// </summary>
        //private const float MoveSpeed = 64.0f;

        /// <summary>
        /// Constructs a new Enemy.
        /// </summary>
        public Enemy(SpriteSheet spriteSheet, Level level, Vector2 position, string[] idleSpriteSet, string[] runSpriteSet)
        {
            this.level = level;
            this.position = position;

            LoadContent(spriteSheet, idleSpriteSet, runSpriteSet);
        }

        /// <summary>
        /// Loads a particular enemy sprite sheet and sounds.
        /// </summary>
        public void LoadContent(SpriteSheet spriteSheet, string[] idleSpriteSet, string[] runSpriteSet)
        {
			// Load animations.
            idleAnimation = new Animation(Vector2.Zero, TimeSpan.FromSeconds(0.1f), SpriteEffects.None, idleSpriteSet, true);
            runAnimation = new Animation(Vector2.Zero, TimeSpan.FromSeconds(0.15f), SpriteEffects.None, runSpriteSet, true);
            sprite = new AnimationPlayer(spriteSheet, position, idleAnimation);            // Load animated textures.

            // Calculate bounds within texture size.
            int width = (int)(sprite.CurrentSprite.Rectangle.Width * 0.35f);
            int left = (sprite.CurrentSprite.Rectangle.Width - width) / 2;
            int height = (int)(sprite.CurrentSprite.Rectangle.Width * 0.7f);
            int top = sprite.CurrentSprite.Rectangle.Height - height;
            localBounds = new Rectangle(left, top, width, height);
        }


        /// <summary>
        /// Paces back and forth along a platform, waiting at either end.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate tile position based on the side we are walking towards.
            float posX = Position.X + localBounds.Width / 2 * (int)direction;
            int tileX = (int)Math.Floor(posX / TileInfo.Width) - (int)direction;
            int tileY = (int)Math.Floor(Position.Y / TileInfo.Height);

            if (waitTime > 0)
            {
                // Wait for some amount of time.
                waitTime = Math.Max(0.0f, waitTime - (float)gameTime.ElapsedGameTime.TotalSeconds);
                if (waitTime <= 0.0f)
                {
                    // Then turn around.
                    direction = (FaceDirection)(-(int)direction);
                }
            }
            else
            {
                // If we are about to run into a wall or off a cliff, start waiting.
                if (Level.GetCollision(tileX + (int)direction, tileY - 1) == TileCollision.Impassable ||
                    Level.GetCollision(tileX + (int)direction, tileY) == TileCollision.Passable)
                {
                    waitTime = MaxWaitTime;
                }
                else
                {
                    // Move in the current direction.
                    Vector2 velocity = new Vector2((int)direction * EnemyInfo.Speed * elapsed, 0.0f);
                    position = position + velocity;
                }
            }
        }

        /// <summary>
        /// Draws the animated enemy.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteRender spriteRender)
        {
            var animation = idleAnimation;
            
            // Stop running when the game is paused or before turning around.
            if (!Level.Player.IsAlive ||
                Level.ReachedExit ||
                Level.TimeRemaining == TimeSpan.Zero ||
                waitTime > 0)
            {
                animation = idleAnimation;
            }
            else
            {
                animation = runAnimation;
            }
            
			sprite.PlayAnimation(animation);

            // Draw facing the way the enemy is moving.
            SpriteEffects flip = direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            spriteRender.Draw( sprite.CurrentSprite, Position, GemInfo.Color, 0.0f, 1.0f, flip);
        }
    }
}
