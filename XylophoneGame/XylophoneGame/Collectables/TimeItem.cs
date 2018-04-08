using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace XylophoneGame
{
    public class TimeItem: Circle
    {
        private AnimationPlayer AnimationPlayer;
        
        public bool IsEnabled { get; private set; }
        
        public TimeItem(String name, Vector2 position, int radius, float speed, float jump, float mass, Color color, SoundEffect note, Texture2D texture = null, Texture2D animationTexture = null, TileCollision collision = TileCollision.Passable)
        : base(name, position, radius, speed, jump, mass, color, note, texture, collision)
        {
            AnimationPlayer.PlayAnimation(new Animation(animationTexture, Position, 0.4f, 21, 60, Color, true));
            IsEnabled = true;
        }

		public override void UpdatePosition(GameTime gameTime, Vector2 mapSize)
		{
			base.UpdatePosition(gameTime, mapSize);

            if (IsEnabled)
            {
                AnimationPlayer.Update(gameTime, Position);
            }
		}

        public void Disable()
        {
            IsEnabled = false;
        }
        
        
		public override void Draw(SpriteBatch spriteBatch, Rectangle screenSafeArea) 
        {
            if (IsEnabled)
            {
                if (AnimationPlayer.Animation == null || AnimationPlayer.Animation.Active == false)
                {
                    base.Draw(spriteBatch, screenSafeArea);
                }
                else
                {
                    AnimationPlayer.Draw(spriteBatch);
                }
            }
        }

    }
}
