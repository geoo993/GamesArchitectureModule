using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XylophoneGame
{
    public class WinScreen: Screen
    {
        public ScoreObserver Observer;
        
        public WinScreen( ScreensType type, ScreenManager parent, ContentManager contentManager)
        : base(type, parent, contentManager)
        {
            Observer = new ScoreObserver("Score Observer");
			Observer.Subscribe(parent.ScoreSubject);
            
            SaveLoadJSON.Save(new GameData(Parent.Player, Parent.Level, Observer.Matches, true));
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
        
        public override void Destroy()
        {
          
            Observer.OnCompleted();
            Observer = null;
            
            base.Destroy();
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
        public override void Update(GameTime gameTime, Vector2 screenCenter)
        {
            base.Update(gameTime, screenCenter);
            
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Draw(SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
            base.Draw(spriteBatch, screenSafeArea);
            
            var word = "Well done "+ Parent.Player+", you played all "+ Observer.Matches.ToString() +" notes";
            Parent.ShowResults(spriteBatch, screenSafeArea, word, false);
        }
    }
}
