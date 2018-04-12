using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XylophoneGame
{
    public class LoseScreen: Screen
    {
        public ScoreObserver Observer;
        
        public LoseScreen(ScreensType type, ScreenManager parent, ContentManager contentManager)
        : base(type, parent, contentManager)
        {
            Observer = new ScoreObserver("Lose Screen Observer");
            Observer.Subscribe(parent.ScoreSubject);
        }
      
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Construct(Color backgroundColor, Texture2D backgroundTexture)
        {
            base.Construct(backgroundColor, backgroundTexture);
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void OnEnter()
        {
            base.OnEnter();
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void OnExit()
        {
            Destroy();
            base.OnExit();
        }
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Destroy()
        {
            
            Observer.OnCompleted();
            Observer = null;
            
            base.Destroy();
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            Debug.Print("");
            Debug.Print("Score is " + Observer.Matches);
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Draw(SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
        
             base.Draw(spriteBatch, screenSafeArea);
             
            //HudPosition = new Vector2(GameInfo.Camera.Position.X, GameInfo.Camera.Position.Y - (safeArea.Height / 2.0f));
            //var notesPosition = HudPosition + new Vector2(-(Width * 0.5f), 0.0f);
            
            //var Max = MaxNotes - 1;
            //var word = Matches.ToString() +"/"+Max.ToString();
            //var width = HudLargeFont.MeasureString(word).X;
            //var color = HudTextColor * FlashScoreCount;
            //var origin = new Vector2(width / 2, 0);
            //DrawShadowedString(spriteBatch, "", word, GameInfo.Camera.Position - new Vector2(0.0f, 100.0f), origin, color, 1.0f);
            
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Vector2 origin, Color color, float scale)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), color, 0.0f, origin, scale, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, value, position, color, 0.0f, origin, scale, SpriteEffects.None, 0.0f);
        }
        
        
    }
}
