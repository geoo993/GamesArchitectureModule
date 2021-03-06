﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XylophoneGame
{
    public class MainScreen: Screen
    {
		
        public MainScreen(ScreensType type, ScreenManager parent, ContentManager contentManager)
        : base(type, parent, contentManager)
        {
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
        }
        
    }
}
