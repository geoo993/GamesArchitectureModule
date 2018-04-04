using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WareHouse3
{
    public class SplashScreen: Screen
    {
        /// <summary>
        /// The count down the time for the splash screen to end.
        /// </summary>
        public int CountDown;
        
        public SplashScreen(ScreensType type, ScreenManager parent, ContentManager contentManager, int coundown)
        : base(type, parent, contentManager)
        {
            CountDown = coundown;
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
            base.Destroy();
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(TimeSpan currentGameTime)
        {
            base.Update(currentGameTime);
            
            if (CountDown <= 0) {
                Parent.SetState(ScreensState.MAIN);
            }
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Draw(TimeSpan currentGameTime, SpriteBatch spriteBatch, Vector2 cameraLocation)
        {
            base.Draw(currentGameTime, spriteBatch, cameraLocation);

        }
    }
}
