using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace XylophoneGame
{
    public class MusicNoteItem: Circle
    {
        private AnimationPlayer AnimationPlayer;
        
        /// <summary>
        /// explosion particle of the ball
        /// </summary>
        private Particles Particles;
        
        public bool IsEnabled { get; private set; }
        
        public MusicNoteItem(String name, Vector2 position, int radius, float speed, float jump, float mass, Color color, SoundEffect note, Texture2D texture = null, Texture2D animationTexture = null, Texture2D particlesTexture = null, TileCollision collision = TileCollision.Passable)
        : base(name, position, radius, speed, jump, mass, color, note, texture, collision)
        {
            Particles = new Particles(Color.Black, 20, radius, 0.015f, particlesTexture);
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

            Particles.UpdateExplosionParticles();
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

            Particles.DrawExplosionParticles(spriteBatch, screenSafeArea);
            
        }
        
        public void Disable()
        {
            IsEnabled = false;
            Particles.AddExplosionParticle(Position, Radius, false);
        }
     
        public override void Destroy () {
            AnimationPlayer.Destroy();
            Particles.Destroy();
            Particles = null;
            
            base.Destroy();
        }

    }
}
