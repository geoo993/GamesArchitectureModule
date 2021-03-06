﻿using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XylophoneGame
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        
        #region Fields
        
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private ScreenManager ScreenManager;
        
        #endregion
        
        public Game1()
        {
        
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Window.Title = "Xylophone Game";

#if WINDOWS_PHONE
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = true;
#else
            // set the backbuffer size to something that will work well on both xbox
            // and windows.
            graphics.PreferredBackBufferWidth = GameInfo.ScreenWidth;
            graphics.PreferredBackBufferHeight = GameInfo.ScreenHeight;
#endif

            //Debug.Print("##########################");
            //Debug.Print("   Initializing game...   ");
            //Debug.Print("##########################");

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
			ScreenManager = new ScreenManager(Window.Title, GameInfo.Player, Content, graphics.GraphicsDevice, Services);
            
            base.Initialize();
        }

        
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Construct();            
        }
        
       
         /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            if (ScreenManager != null)
            {
                ScreenManager.Destroy();
            }
            ScreenManager = null;
            Content.Unload();
        }


        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (ScreenManager.ShouldExit)
            {
                this.Exit();
            }

            ScreenManager.Update(gameTime);
            
            base.Update(gameTime);
        }
        
        
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            
            graphics.GraphicsDevice.Clear(ScreenManager.CurrentScreen.BackgroundColor);

            //spriteBatch.Begin();
			//this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
			this.spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, ScreenManager.Camera.TranslationMatrix );
            {
                ScreenManager.Draw(spriteBatch, GraphicsDevice.Viewport.TitleSafeArea);
            }
            this.spriteBatch.End();

            base.Draw(gameTime);
        }
        
       
    }
}
