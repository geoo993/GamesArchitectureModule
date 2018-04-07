
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
        
		protected Texture2D ProgressSprite { get; set; }
        protected Texture2D TimeProgressSprite { get; set; }
		protected Texture2D BackgroundSprite { get; set; }
        protected Vector2 Position;
        protected Vector2 ScaleFactor;
        protected Vector2 TimeScaleFactor;
        protected Color ProgressColor { get; set; }
        protected Color TimeProgressColor { get; set; }
        protected Color BackgroundColor { get; set; }
        
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
            ProgressSprite = null;
            TimeProgressSprite = null;
            BackgroundSprite = null;
            Position = Vector2.Zero;
            ScaleFactor = Vector2.One;
            TimeScaleFactor = Vector2.One;
            ProgressColor = Color.White;
            TimeProgressColor = Color.Gray;
            BackgroundColor = Color.Black;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Construct(int width, int height, Vector2 position, Color color, Color timeColor, Color background)
        {
            Width = width;
            Height = height;
			ProgressSprite = Texture(width, height, color);
			TimeProgressSprite = Texture(width, 10, timeColor);
			BackgroundSprite = Texture(width, height, background);
			ProgressColor = color;
            ProgressColor = timeColor;
            BackgroundColor = background;
			Position = position;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Destroy()
        {
            ProgressSprite = null;
            TimeProgressSprite = null;
            BackgroundSprite = null;
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
        public void SetTimeScaleFactor(Vector2 scaleFactor)
        {
            TimeScaleFactor = scaleFactor;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetTimeScaleFactorX(float x)
        {
            TimeScaleFactor.X = x;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetTimeScaleFactorY(float y)
        {
            TimeScaleFactor.Y = y;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void IncreaseTimeScaleFactorXBy(float amount)
        {
            TimeScaleFactor.X += amount;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetProgress(float percent)
        {
            //Debug.Print(" ");
            //Debug.Print("Width "+Width.ToString());
            //Debug.Print("Percent Given "+percent.ToString());
            var Progress = MathExtensions.Value(percent, Width, 0) / Width;
            
            //Debug.Print("Percent Calculated "+Progress.ToString());
            SetScaleFactorX(Progress);
        }
        
        public void SetTimeProgress(float progress)
        {
            var TimeProgress = progress;
            SetTimeScaleFactorX(TimeProgress);
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Update(GameTime gameTime)
        {
        
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
            if (ProgressSprite != null)
            {
                
				spriteBatch.Draw(BackgroundSprite, Position, null, BackgroundColor, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                
                spriteBatch.Draw(ProgressSprite,
                    Position,
                    null,
                    ProgressColor,
                    0.0f, // Rotation
                    Vector2.Zero, // Origin
                    ScaleFactor,
                    SpriteEffects.None,
                    0.0f);
                   
                spriteBatch.Draw(TimeProgressSprite, 
                    Position + new Vector2(0, Height), 
                    null, 
                    TimeProgressColor, 
                    0.0f, 
                    Vector2.Zero, 
                    TimeScaleFactor, 
                    SpriteEffects.None, 
                    0.0f);
                    
            }
        }
        
        
    }
}
