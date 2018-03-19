namespace TexturePackerLoader
{
    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class SpriteRender
    {
        private const float ClockwiseNinetyDegreeRotation = (float)(Math.PI / 2.0f);

        private SpriteBatch spriteBatch;

        public SpriteRender (SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }

        // <param name="position">This should be where you want the pivot point of the sprite image to be rendered.</param>
        public void Draw(SpriteFrame sprite, Vector2 position, Color? color = null, float rotation = 0, float scale = 1, SpriteEffects spriteEffects = SpriteEffects.None)
        {
            Color col = Color.White; 
            if (color != null) col = color.Value;
            
            Vector2 origin = sprite.Origin;
            if (sprite.IsRotated)
            {
                rotation -= ClockwiseNinetyDegreeRotation;
                switch (spriteEffects)
                {
                    case SpriteEffects.FlipHorizontally: spriteEffects = SpriteEffects.FlipVertically; break;
                    case SpriteEffects.FlipVertically: spriteEffects = SpriteEffects.FlipHorizontally; break;
                }
            }
            switch (spriteEffects)
            {
                case SpriteEffects.FlipHorizontally: origin.X = sprite.Rectangle.Width - origin.X; break;
                case SpriteEffects.FlipVertically: origin.Y = sprite.Rectangle.Height - origin.Y; break;
            }
            
            // Draw the current frame.
            spriteBatch.Draw(sprite.Texture, position, sprite.Rectangle, col, rotation, origin, new Vector2(scale, scale), spriteEffects, 0.0f);
                 
        }
    }
}