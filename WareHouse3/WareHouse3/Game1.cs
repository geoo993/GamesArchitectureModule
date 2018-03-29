﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Tracing;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using NLua;
using TexturePackerLoader;

namespace WareHouse3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Fields
        private readonly TimeSpan timePerFrame = TimeSpan.FromSeconds(1f/30f); 
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		Vector2 centreScreen;
        Vector2 screenSize;
        
        private Level level;
        
         #endregion
        
        public Game1()
        {
        
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            

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

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Device.graphicsDevice = this.GraphicsDevice;
            GameInfo.Camera.ViewportWidth = graphics.GraphicsDevice.Viewport.Width;
            GameInfo.Camera.ViewportHeight = graphics.GraphicsDevice.Viewport.Height;
            Commands.manager.AddKeyboardBinding(Keys.Escape, StopGame);
            base.Initialize();
        }

        #region Game Actions
        public void StopGame(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                Exit();
            }
        }

        #endregion
        
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            centreScreen = new Vector2 (this.GraphicsDevice.Viewport.Width / 2, this.GraphicsDevice.Viewport.Height / 2);
            screenSize = new Vector2 (this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);

            GameInfo.Camera.SetKeyoardBindings();
            
            LoadLevel();
        }
        
        private void LoadLevel()
        {
            // Unloads the content for the current level before loading the next one.
            if (level != null)
                level.Dispose();

            // Load the level.
            string levelPath = string.Format("Content/Levels/level.txt");
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                level = new Level(Services, fileStream, Commands.manager);
        }
       
         /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update the command manager (updates polling input and fires input events)
            Commands.manager.Update();
            
            var mapSize = new Vector2(GameInfo.MapWidth, GameInfo.MapHeight);
            System.Diagnostics.Debug.Print("Map Size"+mapSize.ToString());
            
            level.Update(gameTime, mapSize);
            
            base.Update(gameTime);
        }
        
        
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            //TODO: Add your drawing code here
            //this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            this.spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, GameInfo.Camera.TranslationMatrix );
            
            level.Draw(gameTime, spriteBatch);
            
            this.spriteBatch.End();

            base.Draw(gameTime);
        
        }
       
    }
}
