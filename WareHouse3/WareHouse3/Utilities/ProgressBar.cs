
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// https://github.com/andrekishimoto/fsm/blob/master/src/AK_Game/Game/Object/GameBar.cs

namespace WareHouse3
{
    //-----------------------------------------------------------------------------
    // GameBar
    //
    // Game bar (1x1 texture that will scale during Draw())
    // TODO: Create 1x1 texture via code. We don't need to use external PNGs
    // since it's a simple 1 pixel color.
    //-----------------------------------------------------------------------------
    public class Progressbar
    {
        
        protected Texture2D BackgroundSprite { get; set; }
        protected Texture2D Sprite { get; set; }
        protected Vector2 Position;
        protected Vector2 ScaleFactor;
        protected Color Color { get; set; }
        protected Color BackgroundColor { get; set; }
        
        protected float Progress { get; set; }
        public double ProgressSpeed { get; set; }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public int Width { get; set; }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public int Height { get; set; }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public Progressbar()
        {
            Width = 1;
            Height = 1;
            ProgressSpeed = 20.0f;
            Sprite = null;
            BackgroundSprite = null;
            Position = Vector2.Zero;
            ScaleFactor = Vector2.One;
            Color = Color.White;
            BackgroundColor = Color.Black;
            Progress = 0.0f;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Construct(int width, int height, Vector2 position, Color color, Color background)
        {
            Width = width;
            Height = height;
            ProgressSpeed = 20.0f;
            Sprite = Texture(width, height, color);//sprite;
			Color = color;
            BackgroundSprite = Texture(width, height, background);
            BackgroundColor = background;
			Position = position;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Destroy()
        {
            Sprite = null;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public Vector2 GetPosition()
        {
            return Position;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetPosition(float x, float y)
        {
            Position.X = x;
            Position.Y = y;
        }

        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetScaleFactor(Vector2 scaleFactor)
        {
            ScaleFactor = scaleFactor;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetScaleFactorX(float x)
        {
            ScaleFactor.X = x;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetScaleFactorY(float y)
        {
            ScaleFactor.Y = y;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void IncreaseScaleFactorXBy(float amount)
        {
            ScaleFactor.X += amount;
        }

        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Update(GameTime gameTime)
        {
            var elapsed = (float)(gameTime.TotalGameTime.TotalSeconds * ProgressSpeed) % Width;
            
            Progress = elapsed / (float)Width;
            SetScaleFactorX(1.0f - Progress);
        }
        
         /// <summary>
        /// texture area of the box
        /// </summary>
        private Texture2D Texture(int width, int height, Color color) {
            
            Texture2D rectangle = new Texture2D(Device.graphicsDevice, width, height, false, SurfaceFormat.Color);
            
            Color[] colorData = new Color[width * height];
            for (int i = 0; i < width * height; i++)
                colorData[i] = color;
            rectangle.SetData<Color>(colorData);
            return rectangle;
        }
       
         //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Draw(SpriteBatch spriteBatch)
        {
            if (Sprite != null)
            {
                
				spriteBatch.Draw(BackgroundSprite, Position, null, BackgroundColor, 0.0f, new Vector2(Width, 0), 1.0f, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(Sprite,
                    Position,
                    null,
                    Color,
                    0.0f, // Rotation
                    new Vector2(Width,0), // Origin
                    ScaleFactor,
                    SpriteEffects.None,
                    0.0f);
                    
            }
        }
        
        
    }
}
