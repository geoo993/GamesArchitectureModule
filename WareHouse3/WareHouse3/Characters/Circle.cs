#region File Description
//-----------------------------------------------------------------------------
// Circle.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

// http://xboxforums.create.msdn.com/forums/t/7414.aspx
// http://rbwhitaker.wikidot.com/monogame-rotating-sprites
// https://stackoverflow.com/questions/43485700/xna-monogame-detecting-collision-between-circle-and-rectangle-not-working
// https://stackoverflow.com/questions/401847/circle-rectangle-collision-detection-intersection
// https://stackoverflow.com/questions/8645910/checking-collision-circle-with-rectangle-with-c-sharp-xna-4-0


using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WareHouse3
{

    /// <summary>
    /// Represents a 2D circle.
    /// </summary>
    public class Circle
    {
        /// <summary>
        /// circle move speed.
        /// </summary>
        private float moveSpeed;
        
        /// <summary>
        /// rotation speed.
        /// </summary>
        //private float rotationSpeed;
        
        /// <summary>
        /// Center position of the circle.
        /// </summary>
        //public Vector2 Origin
        //{
        //    get { return origin; }
        //}
        private Vector2 origin;

        /// <summary>
        /// Radius of the circle.
        /// </summary>
		public int Size {
            get { return radius * 2; }
        }
        public int Radius
        {
            get { return radius; }
        }
        public int radius;

        /// <summary>
        /// Current position of the circle.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }
        private Vector2 position;
        
        /// <summary>
        /// Angle rotation of the circle.
        /// </summary>
        public float Angle
        {
            get { return angle; }
        }
        private float angle = 0f;

        private Rectangle localBounds;
        /// <summary>
        /// Gets a rectangle which bounds this circle in world space.
        /// </summary>
        public Rectangle BoundingRectangle
        {
			get
			{
				int left = (int)Math.Round(Position.X - origin.X) + localBounds.X;
				int top = (int)Math.Round(Position.Y - origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
			}
        }
         /// <summary>
        /// Constructs a new circle.
        /// </summary>
        public Circle(Vector2 position, int radius, float speed)
        {
			this.radius = radius;
            this.origin = Origin.GetOrigin(Size, Size, Origin.FrameOrigin.center);
            this.position = position;
            this.moveSpeed = speed;
            this.angle = 0.3f;
            this.localBounds = new Rectangle((int)this.origin.X, (int)this.origin.Y, Size, Size);
        }
        
        public void SetKeyoardBindings(CommandManager commandManager) 
        {
            commandManager.AddKeyboardBinding(Keys.Left, LeftMovement);
            commandManager.AddKeyboardBinding(Keys.Right, RightMovement);
            commandManager.AddKeyboardBinding(Keys.Up, UpMovement);
            commandManager.AddKeyboardBinding(Keys.Down, DownMovement);
        }
        
        private void LeftMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                position.X -= moveSpeed;
            }
        }
        
        private void RightMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                position.X += moveSpeed;
            }
        }
        
        private void UpMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                position.Y -= moveSpeed;
            }
        }
        
        private void DownMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                position.Y += moveSpeed;
            }
        }
        
        public void UpdatePosition(Vector2 screenSize)
        {
            // Make sure that the player does not go out of bounds
            position.X = MathHelper.Clamp(position.X, origin.X, (origin.X + screenSize.X) - Size);
            position.Y = MathHelper.Clamp(position.Y, origin.Y, (origin.Y + screenSize.Y) - Size);
        
            //System.Diagnostics.Debug.Print(BoundingRectangle.Center.ToString());
            //System.Diagnostics.Debug.Print(origin.ToString());
        }
        
        /// <summary>
        /// texture area of the circle type 1
        /// </summary>
        Texture2D CreateCircle(GraphicsDevice graphicsDevice)
        {
            
            Texture2D texture = new Texture2D(graphicsDevice, Size, Size);
            Color[] colorData = new Color[Size*Size];
        
            float diam = Size / 2f;
            float diamsq = diam * diam;
        
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    int index = x * Size + y;
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }
        
            texture.SetData(colorData);
            return texture;
        }
        
        /// <summary>
        /// texture area of the circle type 2
        /// </summary>
        Texture2D CreateCircle(GraphicsDevice graphicsDevice, Color color)  { 
            Texture2D texture = new Texture2D(graphicsDevice, Size, Size); 
            Color[] c = new Color[texture.Width * texture.Height]; 
            texture.GetData<Color>(c); 
            DrawCircle(texture.Width / 2, texture.Height / 2, Radius, color, ref c, texture.Width, texture.Height); 
            texture.SetData<Color>(c); 
            return texture; 
        } 
        
        void DrawCircle(int x, int y, int r, Color c, ref Color[] z, int width, int height) 
        { 
            int i, j; 
            for (i = 0; i < 2 * r; i++) 
            { 
                if ((y - r + i) >= 0 && (y - r + i) < height) 
                { 
                    int len = (int)(Math.Sqrt(Math.Cos(0.5f * Math.PI * (i - r) / r)) * r * 2); 
                    int xofs = x - len / 2; 
                    if (xofs < 0) 
                    { 
                        len += xofs; 
                        xofs = 0; 
                    } 
                    if (xofs + len >= width) 
                    { 
                        len -= (xofs + len) - width; 
                    } 
                    int ofs = (y - r + i) * width + xofs; 
                    for (j = 0; j < len; j++) 
                        z[ofs + j] = c; 
                } 
            } 
        } 
        
        /// <summary>
        /// texture area of the line
        /// </summary>
        public static Texture2D CreateLine(GraphicsDevice graphicsDevice, Vector2 a, Vector2 b, Color col) 
        { 
            Texture2D t = new Texture2D(graphicsDevice, 640, 480); 
            Color[] c = new Color[640 * 480]; 
            DrawLine((int)a.X, (int)b.X, (int)a.Y, (int)b.Y, c, 640, col); 
            t.SetData<Color>(c); 
            return t; 
        } 
        static void DrawLine(int x1, int x2, int y1, int y2, Color[] c, int width, Color col) 
        { 
            int deltax = x2 - x1;           // The difference in the x's 
            int deltay = y2 - y1;           // The difference in the y's 
            int y = y1;                     // Start y off at the first pixel value 
            int ynum = deltax / 2;          // The starting value for the numerator 
            for (int x = x1; x <= x2; x++) 
            { 
                c[x + (y * width)] = col; 
                ynum += deltay;           // Increase the numerator by the top of the fraction 
                if (ynum >= deltax)       // Check if numerator >= denominator 
                { 
                    ynum -= deltax;         // Calculate the new numerator value 
                    y++;                    // Increase the value in front of the numerator (y) 
                } 
            } 
        } 
        
        
        /// <summary>
        /// Determines if a circle intersects a rectangle.
        /// </summary>
        /// <returns>True if the circle and rectangle overlap. False otherwise.</returns>
        public bool Intersects(Rectangle rectangle) {
            // Get the rectangle half width and height
            float rW = (rectangle.Width) / 2 ;
            float rH = (rectangle.Height) / 2;
        
            // Get the positive distance. This exploits the symmetry so that we now are
            // just solving for one corner of the rectangle (memory tell me it fabs for 
            // floats but I could be wrong and its abs)
            float distX = Math.Abs(BoundingRectangle.X - rectangle.X);
            float distY = Math.Abs(BoundingRectangle.Y - rectangle.Y);
            
            if(distX >= (Radius + rW) || distY >= (Radius + rH)){
                 // outside see diagram circle E
                 return false;
            }
            if(distX < rW || distY < rH){
                 // inside see diagram circles A and B
                 return true; // touching
            } 
            
        
            // now only circles C and D left to test
            // get the distance to the corner
            distX -= rW;
            distY -= rH;
        
            // Find distance to corner and compare to circle radius (squared and the sqrt root is not needed
            if((distX * distX) + (distY * distY) < Radius * Radius) { 
                // touching see diagram circle C
                return true; 
            }
        
            // Only option left is circle D that is outside             
            return false;

        }
        
        
        
        /// <summary>
        /// Render  circle with sprite batch.
        /// </summary>
        public void Render(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Color color) {
            Texture2D texture = CreateCircle(graphicsDevice); 
            
            spriteBatch.Draw(texture, BoundingRectangle, null, color, MathExtensions.DegreeToRadians(angle), origin, SpriteEffects.None, 0f);
           
        }
        
        
    }
}
