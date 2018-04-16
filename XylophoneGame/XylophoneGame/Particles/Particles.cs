using System;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XylophoneGame
{
    public class Particles
    {

		public List<Circle> ExplosionLayers { get; private set; }
		public List<Circle> TrailLayers { get; private set; }
        private int NumberOfParticles;
        private Texture2D Texture;
        private int Size;
        private float ExplisionSpeed;
        private float TrailSpeed;
        private Color Color;
        
        /// <summary>
        /// Constructs a new circle.
        /// </summary>
        public Particles(Color color, int numberOfParticles = 0, int size = 0, float decay = 0.015f, Texture2D texture = null)
        {
            this.NumberOfParticles = numberOfParticles;
            this.Texture = texture;
            this.Size = size;
            this.ExplisionSpeed = decay;
            this.TrailSpeed = decay;
            this.Color = color;
            this.ExplosionLayers = new List<Circle>();
            this.TrailLayers = new List<Circle>();
        }
        
        #region Trail Particles
        public void AddTrailParticle(Vector2 position, int radius, bool randomColor = true)
        {
            byte red = (byte)GameInfo.Random.Next(0, 255);
            byte green = (byte)GameInfo.Random.Next(0, 255);
            byte blue = (byte)GameInfo.Random.Next(0, 255);
            Color color = randomColor ? new Color(red, green, blue) : Color;
        
            Circle particle = new Circle("", position, radius, 0.0f, 0.0f, 1.0f, color, null, Texture);
            TrailLayers.Add(particle);
        }
        
        public void UpdateTrailParticles() {

            if (TrailLayers.Count > 0) {
            
                for (int i = 0; i < TrailLayers.Count; i++)
                {
                    TrailLayers[i].Opacity -= TrailSpeed;
                    TrailLayers[i].Scale -= TrailSpeed;
                    if (TrailLayers[i].Opacity <= 0.0f || TrailLayers[i].Scale <= 0.001f)
                    {
                        TrailLayers.RemoveAt(i);
                        i--;
                        continue;
                    }
                }
            }
        }
        
        public void DrawTrailParticles(SpriteBatch spriteBatch, Rectangle screenSafeArea, GraphicsDevice graphics)
        {
            
            if (TrailLayers.Count > 0)
            {
                // Draw the particles
                foreach (Circle particle in TrailLayers)
                {
                    particle.Draw(spriteBatch, screenSafeArea, graphics);
                }
            }
        }
        
        #endregion
        
        #region Explosion Particles
        public void AddExplosionParticle(Vector2 position, int size, bool randomColor = true)
        {
            for (int i = 0; i < NumberOfParticles; i++)
            {
                byte red = (byte)GameInfo.Random.Next(0, 255);
                byte green = (byte)GameInfo.Random.Next(0, 255);
                byte blue = (byte)GameInfo.Random.Next(0, 255);
                Color color = randomColor ? new Color(red, green, blue) : Color;
                
                Circle particle = new Circle("", position, GameInfo.Random.Next(size / 4, size), GameInfo.Random.Next(10), 0.0f, 1.0f, color, null, Texture);
                particle.Angle = (float)GameInfo.Random.Next(-180, 180);
                ExplosionLayers.Add(particle);
            }
            
        }
        
        public void UpdateExplosionParticles() {

            if (ExplosionLayers.Count > 0) {
            
                for (int i = 0; i < ExplosionLayers.Count; i++)
                {
                    ExplosionLayers[i].Opacity -= ExplisionSpeed;
                    ExplosionLayers[i].Scale -= ExplisionSpeed;
                    var xSpeed = (int)ExplosionLayers[i].MoveSpeed.X;
                    var ySpeed = (int)ExplosionLayers[i].MoveSpeed.Y;
                    var angleInRadians = MathExtensions.DegreeToRadians(ExplosionLayers[i].Angle);
                    ExplosionLayers[i].Position.X += (float)Math.Cos(angleInRadians) * GameInfo.Random.Next(xSpeed);
                    ExplosionLayers[i].Position.Y += (float)Math.Sin(angleInRadians) * GameInfo.Random.Next(ySpeed);
                    
                    if (ExplosionLayers[i].Opacity <= 0.0f || ExplosionLayers[i].Scale <= 0.001f)
                    {
                        ExplosionLayers.RemoveAt(i);
                        i--;
                        continue;
                    }
            
                }
            } 
        }
        
		public void DrawExplosionParticles(SpriteBatch spriteBatch, Rectangle screenSafeArea, GraphicsDevice graphics)
		{
			if (ExplosionLayers.Count > 0)
			{
				// Draw the particles
				foreach (Circle particle in ExplosionLayers)
				{
					particle.Draw(spriteBatch, screenSafeArea, graphics);
				}
			}
		}
        #endregion
        
        public void Destroy() {
            foreach (Circle particle in TrailLayers){
                particle.Destroy();
            }
            TrailLayers.Clear();
            
            foreach (Circle particle in ExplosionLayers){
                particle.Destroy();
            }
            ExplosionLayers.Clear();

            Texture = null;
        }
        
    }
}
