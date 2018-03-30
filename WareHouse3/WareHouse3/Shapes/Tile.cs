using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace WareHouse3
{
    
    /// <summary>
    /// Controls the collision detection and response behavior of a tile.
    /// </summary>
    public enum TileCollision
    {
        /// <summary>
        /// A passable tile is one which does not hinder player motion at all.
        /// </summary>
        Passable = 0,

        /// <summary>
        /// An impassable tile is one which does not allow the player to move through
        /// it at all. It is completely solid.
        /// </summary>
        Impassable = 1,

        /// <summary>
        /// A platform tile is one which behaves like a passable tile except when the
        /// player is above it. A player can jump up through a platform as well as move
        /// past it to the left and right, but can not fall down through the top of it.
        /// </summary>
        Platform = 2,
    }


    public abstract class Tile
    {
       
        public TileCollision Collision { get; private set; }

        /// <summary>
        /// xylophone sound note of the tile if any
        /// </summary>
        public SoundEffect Note { get; private set; } 
        
        /// <summary>
        /// tile name
        /// </summary>
        public String Name { get; private set; } 
       
        /// <summary>
        /// movement speed of the shape.
        /// </summary>
        public Vector2 MoveSpeed;
        
        /// <summary>
        /// jump speed.
        /// </summary>
        public float JumpSpeed { get; private set; }
        
        /// <summary>
        /// rotation speed.
        /// </summary>
        public float RotationSpeed { get; private set; }
        
        /// <summary>
        /// the speed in which the shape falls down
        /// </summary>
        public readonly Vector2 Gravity = new Vector2(0, 0.45f);

        /// <summary>
        /// the physics movement speed the shape
        /// </summary>
        public Vector2 Velocity;
        public Vector2 Acceleration;
        public float Mass;
        
        /// <summary>
        /// color of the shape texture
        /// </summary>
        public Color Color;
        public Color InitialColor { get; private set; }
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
        protected FrameOrigin.OriginType originType;
        protected Vector2 InitialOrigin;
        public Vector2 Origin { get; set; }
        
        /// <summary>
        /// current motion state of the shape.
        /// </summary>
        public MotionState MotionState;
        
        /// <summary>
        /// Current position of the shape.
        /// </summary>
        public Vector2 Position;
        protected readonly Vector2 InitialPosition;
        
        /// <summary>
        /// determines the mode of the shape
        /// </summary>
        private Vector2 CurrentPosition;
        private Vector2 OldPosition;
        
        
        /// <summary>
        /// the lowest position 
        /// </summary>
        public float Ground;
        
        /// <summary>
        /// the highest position 
        /// </summary>
        public float Ceiling;
        
        /// <summary>
        /// the furthest position to the left
        ///
        public float LeftBoundary;
        
        /// <summary>
        /// the furthest position to the right 
        /// </summary>
        public float RightBoundary;
            
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
                int left = (int)Math.Round(Position.X - Origin.X);// + LocalBounds.X;
                int top = (int)Math.Round(Position.Y - Origin.Y);// + LocalBounds.Y;
                
                return new Rectangle(left, top, LocalBounds.Width, LocalBounds.Height);
            }
        }
        
        protected Tile(String name, Vector2 position, int width, int height, float speed, float jump, float mass, Color color, SoundEffect note = null, Texture2D texture = null, TileCollision collision = TileCollision.Passable)
        {
            this.Name = name;
            this.originType = FrameOrigin.OriginType.center;
            this.Origin = FrameOrigin.GetOrigin(width, height, this.originType);
            this.InitialOrigin = this.Origin;
            this.Position = position;
            this.InitialPosition = position;
            this.LeftBoundary = 0.0f;
            this.RightBoundary = GameInfo.MapWidth;
            this.Ground = GameInfo.MapHeight;
            this.Ceiling = 0.0f;
            this.MoveSpeed = new Vector2(speed, speed);
            this.RotationSpeed = 1.0f;
            this.JumpSpeed = jump;
            this.Velocity = Vector2.Zero;
            this.Acceleration = Vector2.Zero;
            this.Mass = mass;
            this.Angle = 0.0f;
            this.Width = width;
            this.InitialWidth = width;
            this.Height = height;
            this.InitialHeight = height;
            this.Scale = 1.0f;
            this.Depth = 0.0f;
            this.Color = color;
            this.InitialColor = color;
            this.BorderColor = Color.White;
			this.Opacity = 1.0f;
            this.Texture = texture;
            this.HasTexture = (texture != null);
            this.Collision = collision;
            this.Note = note;
            this.MotionState = new MotionState();
            this.LocalBounds = new Rectangle((int)this.Origin.X, (int)this.Origin.Y, width, height);
        }
        
        private void CurrentMode(){
        
            CurrentPosition = Position;
            if (CurrentPosition.Y > OldPosition.Y) {
                MotionState.mode = MotionState.Mode.falling;
            } else if (CurrentPosition.Y < OldPosition.Y) {
                MotionState.mode = MotionState.Mode.jumping;
            }
            else
            {
                MotionState.mode = (CurrentPosition.X < OldPosition.X || CurrentPosition.X > OldPosition.X) ? MotionState.Mode.moving : MotionState.Mode.grounded;
            }
            OldPosition = CurrentPosition;
            
        }
       
        public virtual void UpdatePosition(GameTime gameTime, Vector2 mapSize)
        {
            CurrentMode();
            
            // Make sure that the shape does not go out of bounds
            Position.X = MathHelper.Clamp(Position.X, LeftBoundary + Origin.X, (Origin.X + RightBoundary) - Width);
            Position.Y = MathHelper.Clamp(Position.Y, Ceiling + Origin.Y, (Origin.Y + Ground) - Height);

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
            } 
			spriteBatch.Draw(Texture, BoundingRectangle, null, this.Color * Opacity, MathExtensions.DegreeToRadians(Angle), Vector2.Zero, SpriteEffects.None, Depth);

        }
    }
}
