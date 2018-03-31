using System;
using System.IO;
using System.Collections.Generic;

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
        //private readonly TimeSpan timePerFrame = TimeSpan.FromSeconds(1f/30f); 
         private FrameCounter frameCounter = new FrameCounter();
         private SmoothFramerate smoothFPS = new SmoothFramerate(1000);
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        // Global content.
        private SpriteFont hudFont;
        
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
            Device.graphicsDevice = graphics.GraphicsDevice;
			Commands.manager.AddKeyboardBinding(Keys.Escape, StopGame);
            GameInfo.Camera.SetKeyoardBindings();
            
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
            hudFont = Content.Load<SpriteFont>("Fonts/CandyScript");
            
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
            
            level.Update(gameTime, new Vector2(GameInfo.MapWidth, GameInfo.MapHeight));
            
            
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
            
			DrawHud(gameTime);
            level.Draw(gameTime, spriteBatch);

            
            this.spriteBatch.End();

            base.Draw(gameTime);
        
        }
        
        private void DrawHud(GameTime gameTime)
        {
            Rectangle titleSafeArea = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudLocation = new Vector2(GameInfo.Camera.Position.X - 100, GameInfo.Camera.Position.Y - (titleSafeArea.Height / 2.0f) + 10);
            Color hudColor = Color.Yellow;
            
            // Draw time remaining. Uses modulo division to cause blinking when the
            // player is running out of time.
            string timeString = "Play Xylophone song";
            Color timeColor = Color.Yellow;
            
            DrawShadowedString(hudFont, timeString, hudLocation, timeColor, hudColor);

            // Draw score
            float timeHeight = hudFont.MeasureString(timeString).Y;
            DrawShadowedString(hudFont, "SCORE: ", hudLocation + new Vector2(0.0f, timeHeight * 1.2f), hudColor, hudColor);
            
            
            frameCounter.Update(gameTime);
            smoothFPS.Update(gameTime);
    
            var fps = string.Format("FPS: {0}", frameCounter.AverageFramesPerSecond);
            var fps2 = string.Format("Smooth FPS: {0}", smoothFPS.Framerate);
    
            DrawShadowedString(hudFont, fps, new Vector2(hudLocation.X + 100 - (titleSafeArea.Width / 2.0f), hudLocation.Y), hudColor, hudColor);
            DrawShadowedString(hudFont, fps, new Vector2(hudLocation.X + 100 - (titleSafeArea.Width / 2.0f), hudLocation.Y + (timeHeight * 1.2f)), hudColor, hudColor);
        }

        private void DrawShadowedString(SpriteFont font, string value, Vector2 position, Color color, Color borderColor)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), borderColor);
            spriteBatch.DrawString(font, value, position, color);
        }
       
    }
}
