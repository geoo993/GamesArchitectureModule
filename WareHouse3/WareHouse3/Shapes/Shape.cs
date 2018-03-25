using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WareHouse3
{
    public abstract class Shape
    {
       
        /// <summary>
        /// color of the shape texture
        /// </summary>
        public Color Color;
        public Color BorderColor;
        public float Opacity;
        public float Depth;
        
        /// <summary>
        /// texture of the shape
        /// </summary>
        public Texture2D Texture { get; set; }
        protected bool HasTexture;
        
        /// <summary>
        /// Center position of the shape.
        /// </summary>
        //public Vector2 Origin
        //{
        //    get { return origin; }
        //}
        protected FrameOrigin.OriginType originType;
        private Vector2 InitialOrigin;
        public Vector2 Origin { get; private set;  }

        /// <summary>
        /// Current position of the shape.
        /// </summary>
        public Vector2 Position;
        protected readonly Vector2 InitialPosition;
        
        /// <summary>
        /// Angle rotation of the shape.
        /// </summary>
        public float Angle { get; set; }
        
        /// <summary>
        /// Width of the shape.
        /// </summary>
        private float InitialWidth;
        public int Width { get; private set; }

        /// <summary>
        /// Width of the shape.
        /// </summary>
        private float InitialHeight;
        public int Height { get; private set; }

        /// <summary>
        /// Width of the shape.
        /// </summary>
        public float Scale;
        
        protected Rectangle LocalBounds;
        /// <summary>
        /// Gets a rectangle which bounds this box in world space.
        /// </summary>
        public Rectangle BoundingRectangle
        {
            get
            {
                this.Width = (int)(InitialWidth * Scale);
                this.Height = (int)(InitialHeight * Scale);
                this.Origin = InitialOrigin * Scale;
                this.LocalBounds = new Rectangle((int)this.Origin.X, (int)this.Origin.Y, Width, Height);
                int left = (int)Math.Round(Position.X - Origin.X) + LocalBounds.X;
                int top = (int)Math.Round(Position.Y - Origin.Y) + LocalBounds.Y;
                
                return new Rectangle(left, top, LocalBounds.Width, LocalBounds.Height);
            }
        }
        
        protected Shape(Vector2 position, int width, int height, Color color, Texture2D texture = null)
        {
            this.originType = FrameOrigin.OriginType.center;
            this.Origin = FrameOrigin.GetOrigin(width, height, this.originType);
            this.InitialOrigin = this.Origin;
            this.Position = position;
            this.InitialPosition = position;
            this.Angle = 0.0f;
            this.Width = width;
            this.InitialWidth = width;
            this.Height = height;
            this.InitialHeight = height;
            this.Color = color;
            this.Opacity = 1.0f;
            this.Scale = 1.0f;
            this.Depth = 0.0f;
            this.BorderColor = Color.White;
            this.Texture = texture;
            this.HasTexture = (texture != null);
            this.LocalBounds = new Rectangle((int)this.Origin.X, (int)this.Origin.Y, width, height);
        }
        
        public void SetKeyoardBindings() 
        {
            Commands.manager.AddKeyboardBinding(Keys.Left, LeftMovement);
            Commands.manager.AddKeyboardBinding(Keys.Right, RightMovement);
            Commands.manager.AddKeyboardBinding(Keys.Up, UpMovement);
            Commands.manager.AddKeyboardBinding(Keys.Down, DownMovement);
			Commands.manager.AddKeyboardBinding(Keys.Space, JumpMovement);
            Commands.manager.AddKeyboardBinding(Keys.A, RotateForward);
            Commands.manager.AddKeyboardBinding(Keys.D, RotateBackward);
        }
        
        protected virtual void LeftMovement(ButtonAction buttonState, Vector2 amount)
        {
            
        }
        
        protected virtual void RightMovement(ButtonAction buttonState, Vector2 amount)
        {
           
        }
        
        protected virtual void UpMovement(ButtonAction buttonState, Vector2 amount)
        {
            
        }
        
        protected virtual void DownMovement(ButtonAction buttonState, Vector2 amount)
        {
           
        }
        
        protected virtual void JumpMovement(ButtonAction buttonState, Vector2 amount)
        {
           
        }
        
        protected virtual void RotateForward(ButtonAction buttonState, Vector2 amount)
        {
           
        }
        
        protected virtual void RotateBackward(ButtonAction buttonState, Vector2 amount)
        {
            
        }
        
        public virtual void UpdatePosition(GameTime gameTime, Vector2 screenSize)
        {
            this.BorderColor = this.Color;   
        }

        public virtual bool Intersects(Rectangle rectangle)
        {
            return false;
        }
        
        protected virtual Texture2D CreateTexture() 
        {
            
            return Texture;
        }
        
        
        public virtual void Render(SpriteBatch spriteBatch)
        {
            if (HasTexture == false)
            {
                Texture = CreateTexture();
                //spriteBatch.Draw(Texture, Position, null, this.Color * Opacity, MathExtensions.DegreeToRadians(Angle), Origin, Scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(Texture, BoundingRectangle, null, this.Color * Opacity, MathExtensions.DegreeToRadians(Angle), Origin, SpriteEffects.None, Depth);
            } else {
                var newOrigin = FrameOrigin.GetOrigin(Texture.Width, Texture.Height, this.originType);
                //spriteBatch.Draw(Texture, Position, null, this.Color * Opacity, MathExtensions.DegreeToRadians(Angle), newOrigin, Scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(Texture, BoundingRectangle, null, this.Color * Opacity, MathExtensions.DegreeToRadians(Angle), newOrigin, SpriteEffects.None, Depth);
            }

        }
    }
}
