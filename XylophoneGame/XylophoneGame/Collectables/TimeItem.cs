using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace XylophoneGame
{
    public class TimeItem: Circle
    {
        private AnimationPlayer AnimationPlayer;
        /// <summary>
        /// trail particle of the ball
        /// </summary>
        private List<Circle> Particles;
        private int TotalParticles;
        
        public bool IsEnabled { get; private set; }
        
        public TimeItem(String name, Vector2 position, int radius, float speed, float jump, float mass, Color color, SoundEffect note, Texture2D texture = null, Texture2D animationTexture = null, TileCollision collision = TileCollision.Passable)
        : base(name, position, radius, speed, jump, mass, color, note, texture, collision)
        {
            Particles = new List<Circle>();
            TotalParticles = 10;
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
            
            UpdateParticles();
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

            DrawParticles(spriteBatch, screenSafeArea);
            
        }
        
		public void Disable()
		{
			IsEnabled = false;
            AddParticle();
		}
        
        private void AddParticle()
        {
            for (int i = 0; i < TotalParticles; i++)
            {
				byte red = (byte)GameInfo.Random.Next(0, 255);
				byte green = (byte)GameInfo.Random.Next(0, 255);
				byte blue = (byte)GameInfo.Random.Next(0, 255);
				Color col =  new Color(red, green, blue);
				
                Circle particle = new Circle("", Position, GameInfo.Random.Next(Radius/ 4, Radius), GameInfo.Random.Next(10), 0.0f, 1.0f, col, null);
                particle.Angle = (float)GameInfo.Random.Next(-180, 180);
				Particles.Add(particle);
            }
            
        }
        
        private void UpdateParticles() {

            if (Particles.Count > 0) {
            
                for (int i = 0; i < Particles.Count; i++)
                {
                    Particles[i].Opacity -= 0.02f;
                    Particles[i].Scale -= 0.01f;
                    var xSpeed = (int)Particles[i].MoveSpeed.X;
                    var ySpeed = (int)Particles[i].MoveSpeed.Y;
                    var angleInRadians = MathExtensions.DegreeToRadians(Particles[i].Angle);
                    Particles[i].Position.X += (float)Math.Cos(angleInRadians) * GameInfo.Random.Next(xSpeed);
                    Particles[i].Position.Y += (float)Math.Sin(angleInRadians) * GameInfo.Random.Next(ySpeed);
                    
					if (Particles[i].Opacity <= 0.0f || Particles[i].Scale <= 0.001f)
					{
						Particles.RemoveAt(i);
						i--;
						continue;
					}
            
                }
            } 
        }

        private void DrawParticles(SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
            if (Particles.Count > 0)
            {
                // Draw the particles
                foreach (Circle particle in Particles)
                {
                    particle.Draw(spriteBatch, screenSafeArea);
                }
            
            }
        }
        
        public override void Destroy () {
            foreach (Circle particle in Particles){
                particle.Destroy();
            }
            Particles.Clear();

            AnimationPlayer.Destroy();
            
            base.Destroy();
        }

    }
}
