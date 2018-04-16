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

            SaveLoadJSON.Save(new GameData(Parent.Player, Parent.Level, Observer.Matches, false));
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

            var word = "Unlucky "+ Parent.Player+", you played "+ Observer.Matches.ToString() +" notes";
            Parent.ShowResults(spriteBatch, screenSafeArea, word, false);
        }

        
    }
}
