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
// https://gamedev.stackexchange.com/questions/82324/xna-how-to-change-the-sprite-texture-to-white-color
// https://stackoverflow.com/questions/5641579/xna-draw-a-filled-circle
// https://github.com/craftworkgames/MonoGame.Extended/blob/develop/Source/MonoGame.Extended/ShapeExtensions.cs
// https://gamedev.stackexchange.com/questions/61737/resize-texture-in-code

using System;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
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
        PrimitiveLine circleBorder;
        /// <summary>
        /// color of the box texture
        /// </summary>
        public Color color;
        public Color borderColor;
        
        /// <summary>
        /// texture of the circle
        /// </summary>
        public Texture2D texture { get; private set; }
        private bool hasTexture;
        
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
        private Origin.FrameOrigin originType;
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
        private readonly Vector2 initialPosition;
        
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
        public Circle(Vector2 position, int radius, float speed, Color color, Texture2D texture = null)
        {
			this.radius = radius;
            this.originType = Origin.FrameOrigin.center;
            this.origin = Origin.GetOrigin(Size, Size, this.originType);
            this.texture = texture;
            this.hasTexture = (texture != null);
            this.color = color;
            this.borderColor = Color.White;
            this.position = position;
            this.initialPosition = position;
            this.moveSpeed = speed;
            this.angle = 0.3f;
            this.localBounds = new Rectangle((int)this.origin.X, (int)this.origin.Y, Size, Size);
            
            circleBorder = new PrimitiveLine(Device.graphicsDevice, borderColor);
            circleBorder.CreateCircle(radius, 20);
        }
        
        public void SetKeyoardBindings() 
        {
            Commands.manager.AddKeyboardBinding(Keys.Left, LeftMovement);
            Commands.manager.AddKeyboardBinding(Keys.Right, RightMovement);
            Commands.manager.AddKeyboardBinding(Keys.Up, UpMovement);
            Commands.manager.AddKeyboardBinding(Keys.Down, DownMovement);
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
            circleBorder.Position = position;
            //System.Diagnostics.Debug.Print(BoundingRectangle.Center.ToString());
            //System.Diagnostics.Debug.Print(origin.ToString());
        }
        
        
        /// <summary>
        /// texture area of the circle type 1
        /// </summary>
        private Texture2D CreateCircle()
        {
            Texture2D pixels = new Texture2D(Device.graphicsDevice, Size, Size);
            Color[] colorData = new Color[Size * Size];
            
            float diam = Radius;
            float diamsq = diam * diam;
            
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    
                    int index = x * Size + y;
                    
                    Vector2 pos = new Vector2(x - diam, y - diam);
                    
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = this.color;
                    } else {
                        colorData[index] = Color.Transparent;
                    }
                }
            }
        
            pixels.SetData(colorData);
            return pixels;
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
        public void Render(SpriteBatch spriteBatch) {
            
            
            if (hasTexture == false)
            {
				this.texture = CreateCircle();
                spriteBatch.Draw(texture, BoundingRectangle, null, this.color, MathExtensions.DegreeToRadians(angle), origin, SpriteEffects.None, 0f);

            } else {
                var newOrigin = Origin.GetOrigin(texture.Width, texture.Height, this.originType);
                spriteBatch.Draw(texture, BoundingRectangle, null, this.color, MathExtensions.DegreeToRadians(angle), newOrigin, SpriteEffects.None, 0f);
            }
            
            circleBorder.Render(spriteBatch, 2.0f, this.borderColor);
        }
        
        
    }
}
