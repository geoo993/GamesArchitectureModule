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
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace WareHouse3
{
    
    /// <summary>
    /// Represents a 2D circle.
    /// </summary>
    public class Circle: Tile
    {
		/// <summary>
		/// circle border 
		/// </summary>
        private PrimitiveLine CircleBorder;
        public bool IsBorderEnabled { get; set; }

        /// <summary>
        /// Radius of the circle.
        /// </summary>
		public int Size {
            get { return Radius * 2; }
        }
        public int Radius { get; private set; }
        
        /// <summary>
        /// Constructs a new circle.
        /// </summary>
        public Circle(String name, Vector2 position, int radius, float speed, float jump, float mass, Color color, SoundEffect note, Texture2D texture = null, TileCollision collision = TileCollision.Passable)
        : base(name, position, radius * 2, radius * 2, speed, jump, mass, color, note, texture, collision)
        {
			this.Radius = radius;
            this.IsBorderEnabled = false;
            this.CircleBorder = new PrimitiveLine(Device.graphicsDevice, BorderColor);
            this.HasFSM = false;
        }
     
        public override void UpdatePosition(GameTime gameTime, Vector2 mapSize)
        {
			base.UpdatePosition(gameTime, mapSize);
            
            Radius = this.Width / 2;
            if (IsBorderEnabled)
            {
                CircleBorder.CreateCircle(Radius, 20);
                CircleBorder.Position = Position;
            }
        }
        
        /// <summary>
        /// texture area of the circle type 1
        /// </summary>
        protected override Texture2D CreateTexture()
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
                        colorData[index] = this.Color;
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
        public override bool Intersects(Rectangle rectangle) {

            // Get the rectangle half width and height
            float rW = (rectangle.Width) / 2 ;
            float rH = (rectangle.Height) / 2;
        
            // Get the positive distance. This exploits the symmetry so that we now are
            // just solving for one corner of the rectangle (memory tell me it fabs for 
            // floats but I could be wrong and its abs)
            float distX = Math.Abs(BoundingRectangle.Center.X - rectangle.Center.X);
            float distY = Math.Abs(BoundingRectangle.Center.Y - rectangle.Center.Y);
            
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
        
        public bool IntersectsCircle(Rectangle rectangle)
        {
            // https://stackoverflow.com/questions/24559585/how-to-create-a-circle-variable-in-monogame-and-detect-collision-with-other-circ
            // the first thing we want to know is if any of the corners intersect
            var corners = new[]
            {
                new Point(rectangle.Top, rectangle.Left),
                new Point(rectangle.Top, rectangle.Right),
                new Point(rectangle.Bottom, rectangle.Right),
                new Point(rectangle.Bottom, rectangle.Left)
            };
    
            foreach (var corner in corners)
            {
                if (ContainsPoint(corner))
                    return true;
            }
    
            // next we want to know if the left, top, right or bottom edges overlap
            if (BoundingRectangle.Center.X - Radius > rectangle.Right || BoundingRectangle.Center.X + Radius < rectangle.Left)
                return false;
    
            if (BoundingRectangle.Center.Y - Radius > rectangle.Bottom || BoundingRectangle.Center.Y + Radius < rectangle.Top)
                return false;
    
            return true;
        }
    
        public bool Intersects(Circle circle)
        {
            // put simply, if the distance between the circle centre's is less than
            // their combined radius
            var centre0 = new Vector2(circle.BoundingRectangle.Center.X, circle.BoundingRectangle.Center.Y);
            var centre1 = new Vector2(BoundingRectangle.Center.X, BoundingRectangle.Center.Y);
            return Vector2.Distance(centre0, centre1) < Radius + circle.Radius;
        }
    
        public bool ContainsPoint(Point point)
        {
            var vector2 = new Vector2(point.X - BoundingRectangle.Center.X, point.Y - BoundingRectangle.Center.Y);
            return vector2.Length() <= Radius;
        }
        
        /*
        //https://stackoverflow.com/questions/8645910/checking-collision-circle-with-rectangle-with-c-sharp-xna-4-0
        public bool Intersects(Rectangle rectangle)
        {
            Vector2 v = new Vector2(MathHelper.Clamp(BoundingRectangle.Center.X, rectangle.Left, rectangle.Right),
                                    MathHelper.Clamp(BoundingRectangle.Center.Y, rectangle.Top, rectangle.Bottom));
    
            Vector2 direction = BoundingRectangle.Center - v;
            float distanceSquared = direction.LengthSquared();
    
            return ((distanceSquared > 0) && (distanceSquared < Radius * Radius));
        }
        */
        
        /// <summary>
        /// Render  circle with sprite batch.
        /// </summary>
        public override void Render(SpriteBatch spriteBatch) {

            base.Render(spriteBatch);

            if (IsBorderEnabled)
            {
                CircleBorder.Render(spriteBatch, 2.0f, this.BorderColor * Opacity);
            }
        }
        
        
        public static void UpdateObstaclesMovement(List<Circle> tiles) 
        {   
            if (tiles.Count <= 0 ) {
                return;
            }
            
            Circle tempBall1;            
            Circle tempBall2;
            
            
            for (int i = 0; i < tiles.Count; i++)
            {
                
                tempBall1 = tiles[i];

                for (int k = 0; k < tiles.Count; k++)
                {
                    tempBall2 = tiles[k];
                    
                    if (tempBall1 == tempBall2) continue;

                    if (CircularCollision(tempBall1, tempBall2))
                    {
                        CollideBalls(tempBall1, tempBall2);
                        
                        if(CircularCollision(tempBall1,tempBall2))
                        {
                            
                            tempBall1.MoveSpeed *= -1;
                            tempBall2.MoveSpeed *= -1;
                            
                            // go in different directions
                            tempBall1.Position += tempBall1.MoveSpeed;
                            
                            tempBall2.Position -= tempBall2.MoveSpeed;
                        }
                        
                    }
                }

            }
            
        }
        
        public static void CollideBalls(Circle circle1, Circle circle2)
        {
            float dx = circle1.Position.X - circle2.Position.X;
            float dy = circle1.Position.Y - circle2.Position.Y;
            float collisionAngle = (float)Math.Atan2(dy, dx);
            
            var speed1 = Math.Sqrt( (circle1.MoveSpeed.X * circle1.MoveSpeed.X) + (circle1.MoveSpeed.Y * circle1.MoveSpeed.Y));
            var speed2 = Math.Sqrt( (circle2.MoveSpeed.X * circle2.MoveSpeed.X) + (circle2.MoveSpeed.Y * circle2.MoveSpeed.Y));
            
            var direction1 = Math.Atan2(circle1.MoveSpeed.Y, circle1.MoveSpeed.X);
            var direction2 = Math.Atan2(circle2.MoveSpeed.Y, circle2.MoveSpeed.X);
            
            var velocityX1 = speed1 * Math.Cos(direction1 - collisionAngle);
            var velocityY1 = speed1 * Math.Sin(direction1 - collisionAngle);         
            var velocityX2 = speed2 * Math.Cos(direction2 - collisionAngle);
            var velocityY2 = speed2 * Math.Sin(direction2 - collisionAngle);
            
            var mass1 = (circle1.Width / 2);
            var mass2 = (circle2.Width / 2);
            
            var finalVelocityX1 = ((mass1 - mass2) * velocityX1 + (mass2 + mass2) * velocityX2) / (mass1 + mass2);
            var finalVelocityX2 = ((mass1 + mass1) * velocityX1 + (mass2 - mass1) * velocityX2) / (mass1 + mass2);
            
            var finalVelocityY1 = velocityY1;
            var finalVelocityY2 = velocityY2;
            
            circle1.MoveSpeed.X = (float)(Math.Cos(collisionAngle) * finalVelocityX1 + Math.Cos(collisionAngle + Math.PI / 2) * finalVelocityY1);
            circle1.MoveSpeed.Y = (float)(Math.Sin(collisionAngle) * finalVelocityX1 + Math.Sin(collisionAngle + Math.PI / 2) * finalVelocityY1);
            circle2.MoveSpeed.X = (float)(Math.Cos(collisionAngle) * finalVelocityX2 + Math.Cos(collisionAngle + Math.PI / 2) * finalVelocityY2);
            circle2.MoveSpeed.Y = (float)(Math.Sin(collisionAngle) * finalVelocityX2 + Math.Sin(collisionAngle + Math.PI / 2) * finalVelocityY2);
            
            circle1.Position += circle1.MoveSpeed;
            circle2.Position += circle2.MoveSpeed;
        }
     
        public static bool CircularCollision(Circle mc1, Circle mc2)
        {
            var radiusOfBoth = mc1.Radius + mc2.Radius;
            var distance = MathExtensions.GetDistance(mc1.Position, mc2.Position);
            
            return (distance <= radiusOfBoth);
        }
        
        public override void Destroy()
        {
            CircleBorder.Destroy();
            CircleBorder = null;
            base.Destroy();
        }
        
    }
}
