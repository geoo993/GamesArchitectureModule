namespace TexturePackerLoader
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class SpriteFrame
    {
        public enum FrameOrigin {
            center,
            topleft,
            topcenter,
            bottomcenter,
        }
        
        public SpriteFrame(Texture2D texture, Rectangle sourceRect, Vector2 size, Vector2 pivotPoint, bool isRotated)
        {
            this.Texture = texture;
            this.Rectangle = sourceRect;
            this.Size = size;
            this.PivotPoint = pivotPoint;
			this.IsRotated = isRotated;
            this.originType = FrameOrigin.topleft;
        }

        public Texture2D Texture { get; private set; }
        
        /// <summary>
        /// Gets the frame in the sprite.
        /// </summary>
        public Rectangle Rectangle { get; private set; }
        
        public Vector2 Size { get; private set; }
        
        public Vector2 PivotPoint { get; private set; }

        public bool IsRotated { get; private set; }

        public Vector2 Origin { get; private set; }
       
        private FrameOrigin originType = FrameOrigin.topleft;
        public FrameOrigin OriginType {
            get { return originType; }
            set { 
                originType = value;
                
                switch (originType) {
                    case FrameOrigin.topleft:
                        Origin = IsRotated ? new Vector2(Rectangle.Width * (1 - PivotPoint.Y), Rectangle.Height * PivotPoint.X)
                                           : new Vector2(Rectangle.Width * PivotPoint.X, Rectangle.Height * PivotPoint.Y);
                        break;
                    case FrameOrigin.topcenter:
                        Origin = new Vector2(Rectangle.Width / 2.0f, 0.0f);
                        break;
                    case FrameOrigin.center:
                        Origin = new Vector2(Rectangle.Width / 2.0f, Rectangle.Height / 2.0f);
                        break;
                    case FrameOrigin.bottomcenter:
                        Origin = new Vector2(Rectangle.Width / 2.0f, Rectangle.Height);
                        break;
                }
            }
        }
        
    }
}
