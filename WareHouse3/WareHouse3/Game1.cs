using System;
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
        Random random = new Random(DateTime.Now.Millisecond); 

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		Vector2 centreScreen;
        Vector2 screenSize;
        Ball ball;
        List<Character> obstacles;
        
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
            graphics.PreferredBackBufferWidth = GameInfo.screenWidth;
            graphics.PreferredBackBufferHeight = GameInfo.screenHeight;
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
            
            InitializeControlsBindings();
            base.Initialize();
        }

        private void InitializeControlsBindings()
        {
            Commands.manager.AddKeyboardBinding(Keys.Escape, StopGame);
            
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

            ball = new Ball(new Vector2(100, 560), 20, 8.0f, 5.0f, RandomColor(random));
		    ball.SetKeyoardBindings();
            SetupObstacles();
            //TODO: use this.Content to load your game content here 
        }
        
         /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        
        void SetupObstacles () {
            
            obstacles = new List<Character>();

            for (int i = 0; i < 10; i++) {
				obstacles.Add( new Character(new Vector2(random.Next(0, 600), random.Next(0, 600)), random.Next(50, 200), random.Next(20, 80), (float)random.Next(-20, 20)/20.0f, 10.0f, RandomColor(random)) );
            }
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

            ball.UpdateCollisions(obstacles, screenSize);
			ball.UpdatePosition(gameTime, screenSize);

            foreach (Character obstacle in obstacles)
            {
				obstacle.UpdatePosition(gameTime, screenSize);
            }
            
            // TODO: Add your update logic here
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
            this.spriteBatch.Begin();

            ball.Render(spriteBatch);

            foreach (Character obstacle in obstacles)
            {
				obstacle.Render(spriteBatch);
            }
            
            this.spriteBatch.End();

            base.Draw(gameTime);
        
        }
        
        public Color RandomColor(Random r) {
            byte red = (byte)r.Next(0, 255);
            byte green = (byte)r.Next(0, 255);
            byte blue = (byte)r.Next(0, 255);

            return new Color(red, green, blue);
        }
        
    }
}
