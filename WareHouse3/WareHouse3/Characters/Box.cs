using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WareHouse3
{
    public class Box
    {
        /// <summary>
        /// box move speed.
        /// </summary>
        private float moveSpeed;
        
        /// <summary>
        /// rotation speed.
        /// </summary>
        private float rotationSpeed;
        
        /// <summary>
        /// Center position of the box.
        /// </summary>
        //public Vector2 Origin
        //{
        //    get { return origin; }
        //}
        private Vector2 origin;

        /// <summary>
        /// Width of the box.
        /// </summary>
        public int Width
        {
            get { return width; }
        }
        public int width;
        
        /// <summary>
        /// Width of the box.
        /// </summary>
        public int Height
        {
            get { return height; }
        }
        public int height;
        
        /// <summary>
        /// Current position of the box.
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }
        private Vector2 position;
        
        /// <summary>
        /// Angle rotation of the box.
        /// </summary>
        public float Angle
        {
            get { return angle; }
        }
        private float angle = 0f;

        private Rectangle localBounds;
        /// <summary>
        /// Gets a rectangle which bounds this box in world space.
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


        public Box(Vector2 position, int width, int height, float speed)
        {
            this.origin = Origin.GetOrigin(width, height, Origin.FrameOrigin.center);
            this.position = position;
            this.width = width;
            this.height = height;
            this.moveSpeed = speed;
            this.rotationSpeed = 1.2f;
            this.localBounds = new Rectangle((int)this.origin.X, (int)this.origin.Y, width, height);
        }
        
        public void SetKeyoardBindings(CommandManager commandManager) 
        {
            commandManager.AddKeyboardBinding(Keys.Left, LeftMovement);
            commandManager.AddKeyboardBinding(Keys.Right, RightMovement);
            commandManager.AddKeyboardBinding(Keys.Up, UpMovement);
            commandManager.AddKeyboardBinding(Keys.Down, DownMovement);
            commandManager.AddKeyboardBinding(Keys.A, RotateForward);
			commandManager.AddKeyboardBinding(Keys.D, RotateBackward);
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
        
        private void RotateForward(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                angle += rotationSpeed;
            }
        }
        
        private void RotateBackward(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                angle -= rotationSpeed;
            }
        }
        
        public void UpdatePosition(Vector2 screenSize)
        {
            
            // Make sure that the player does not go out of bounds
            position.X = MathHelper.Clamp(position.X, origin.X, (origin.X + screenSize.X) - Width);
            position.Y = MathHelper.Clamp(position.Y, origin.Y, (origin.Y + screenSize.Y) - Height);
            
        }
        
        /// <summary>
        /// texture area of the box
        /// </summary>
        Texture2D BoxTexture(GraphicsDevice graphicsDevice, Color color) {

            Texture2D rectangle = new Texture2D(graphicsDevice, width, height, false, SurfaceFormat.Color);
            
            Color[] colorData = new Color[width * height];
            for (int i = 0; i < width * height; i++)
                colorData[i] = color;
            rectangle.SetData<Color>(colorData);
            return rectangle;
        }
        
        /// <summary>
        /// Determines if a box intersects a rectangle.
        /// </summary>
        /// <returns>True if the circle and rectangle overlap. False otherwise.</returns>
        public bool Intersects(Rectangle rectangle)
        {
			// Get the rectangle half width and height
			float rW1 = (Width) / 2 ;
			float rH1 = (Height) / 2;
            
            float rW2 = (rectangle.Width) / 2 ;
            float rH2 = (rectangle.Height) / 2;
            
            Rectangle rec1 = new Rectangle((int)(BoundingRectangle.X - rW1), (int)(BoundingRectangle.Y - rH1), BoundingRectangle.Width, BoundingRectangle.Height);
            Rectangle rec2 = new Rectangle((int)(rectangle.X - rW2), (int)(rectangle.Y - rH2), rectangle.Width, rectangle.Height);
            
            return rec1.Intersects(rec2);
        }
        
        
        /// <summary>
        /// Render  box with sprite batch.
        /// </summary>
        public void Render(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Color color) {
            Texture2D texture = BoxTexture(graphicsDevice, color); 
            
            spriteBatch.Draw(texture, BoundingRectangle, null, color, MathExtensions.DegreeToRadians(angle), origin, SpriteEffects.None, 0f);
           
        }
    }
}
