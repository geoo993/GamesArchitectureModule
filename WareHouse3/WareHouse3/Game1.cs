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
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteSheet spriteSheet;
        SpriteRender spriteRender;
        SpriteFrame backgroundSprite;
        Vector2 screenSize;
        Vector2 centreScreen;
        AnimationManager characterAnimationManager;
        CommandManager commandManager;
        
        private int levelIndex = -1;
        private Level level;
        private bool wasContinuePressed;
        private int numberOfLevels = 3;
        
        private GamePadState gamePadState;
        private KeyboardState keyboardState;
        private AccelerometerState accelerometerState;
        
         #endregion
        
        public Game1()
        {
            commandManager = new CommandManager();
            graphics = new GraphicsDeviceManager(this);
            
            //System.Diagnostics.Debug.Print(this.graphics.PreferredBackBufferHeight.ToString());
            //System.Diagnostics.Debug.Print(this.graphics.PreferredBackBufferWidth.ToString());
            
            //graphics.PreferredBackBufferWidth = GameInfo.screenWidth;
            //graphics.PreferredBackBufferHeight = GameInfo.screenHeight;
                        
#if WINDOWS_PHONE
            graphics.IsFullScreen = true;
            TargetElapsedTime = TimeSpan.FromTicks(333333);
#endif
            
            Content.RootDirectory = "Content";
            
            IsMouseVisible = true;
            
            Accelerometer.Initialize();
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
            
            InitializeControlsBindings();
            base.Initialize();
        }

        private void InitializeControlsBindings()
        {
            commandManager.AddKeyboardBinding(Keys.Escape, StopGame);
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
            spriteRender = new SpriteRender(this.spriteBatch);

            var spriteSheetLoader = new SpriteSheetLoader(this.Content);
            spriteSheet = spriteSheetLoader.Load("TexturePackerSprites");
            
            backgroundSprite = this.spriteSheet.Sprite(TexturePackerMonoGameDefinitions.SpriteSheet.Backgrounds_Background);
            centreScreen = new Vector2 (this.GraphicsDevice.Viewport.Width / 2, this.GraphicsDevice.Viewport.Height / 2);

            //LoadNextLevel();
            InitialiseAnimationManager();
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
        
    
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update the command manager (updates polling input and fires input events)
            commandManager.Update();
            
             // Handle polling for our input and handling high-level input
            //HandleInput();
            
            // update our level, passing down the GameTime along with all of our input states
            //level.Update(gameTime, keyboardState, gamePadState, accelerometerState, Window.CurrentOrientation);
            
            // TODO: Add your update logic here
            //characterAnimationManager.Update(gameTime);
            base.Update(gameTime);
        }
        
        private void HandleInput()
        {
            // get all of our input states
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);
            accelerometerState = Accelerometer.GetState();


            bool continuePressed = keyboardState.IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A);

            // Perform the appropriate action to advance the game and
            // to get the player back to playing.
            if (!wasContinuePressed && continuePressed)
            {
                if (!level.Player.IsAlive)
                {
                    level.StartNewLife();
                }
                else if (level.TimeRemaining == TimeSpan.Zero)
                {
                    if (level.ReachedExit)
                        LoadNextLevel();
                    else
                        ReloadCurrentLevel();
                }
            }

            wasContinuePressed = continuePressed;
        }
        
        private void LoadNextLevel()
        {
            var levels = new[] {
                TexturePackerMonoGameDefinitions.Sprites.BackgroundsLayer0Sprites,
                TexturePackerMonoGameDefinitions.Sprites.BackgroundsLayer1Sprites,
                TexturePackerMonoGameDefinitions.Sprites.BackgroundsLayer2Sprites
            };
            
            // move to the next level
            levelIndex = (levelIndex + 1) % numberOfLevels;

            // Unloads the content for the current level before loading the next one.
            if (level != null)
                level.Dispose();

            // Load the level.
            string levelPath = string.Format("Content/Levels/{0}.txt", levelIndex);
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                level = new Level(Services, fileStream, levels[levelIndex], spriteSheet);
        }
        
        private void ReloadCurrentLevel()
        {
            --levelIndex;
            LoadNextLevel();
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
            
            // Draw the background
            //this.spriteRender.Draw(this.backgroundSprite, this.centreScreen);

            // Draw character on screen
            /*
            this.spriteRender.Draw(
                this.spriteSheet.Sprite(
                    TexturePackerMonoGameDefinitions.SpriteSheet.Cowboy_Idle
                ),
                new Vector2(100, 400)
            );
            */
            /*
            // Draw character on screen
            this.spriteRender.Draw(
                this.characterAnimationManager.CurrentSprite, 
                this.characterAnimationManager.CurrentPosition, 
                Color.White, 0, 1,
                this.characterAnimationManager.CurrentSpriteEffects);
            */

            level.Draw(gameTime, spriteRender);
            
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

         private void InitialiseAnimationManager()
        {
#if __IOS__
            var scale = MonoTouch.UIKit.UIScreen.MainScreen.Scale;
            var characterStartPosition = new Vector2(350 * scale, 530 * scale);
            var characterVelocityPixelsPerSecond = 200 * (int)scale;
#else
            var characterStartPosition = new Vector2(0, 530);
            var characterVelocityPixelsPerSecond = 200;
            #endif

            var animationWalkRight = new Animation(new Vector2(characterVelocityPixelsPerSecond, 0), this.timePerFrame, SpriteEffects.None, TexturePackerMonoGameDefinitions.Sprites.capguyWalkSprites);
            var animationWalkLeft = new Animation(new Vector2(-characterVelocityPixelsPerSecond, 0), this.timePerFrame, SpriteEffects.FlipHorizontally, TexturePackerMonoGameDefinitions.Sprites.capguyWalkSprites);
            var animationTurnRightToLeft = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.None, TexturePackerMonoGameDefinitions.Sprites.capguyTurnSprites);
            var animationTurnLeftToRight = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.FlipHorizontally, TexturePackerMonoGameDefinitions.Sprites.capguyTurnSprites);

            var animations = new[] 
            { 
               animationWalkRight//, animationWalkRight, animationWalkRight, animationWalkRight, animationWalkRight, animationWalkRight, animationWalkRight, animationWalkRight, animationWalkRight,
               //animationTurnRightToLeft,
               //animationWalkLeft, animationWalkLeft, animationWalkLeft, animationWalkLeft, animationWalkLeft, animationWalkLeft, animationWalkLeft, animationWalkLeft, animationWalkLeft, 
               //animationTurnLeftToRight
            };

            this.characterAnimationManager = new AnimationManager (this.spriteSheet, characterStartPosition, animations);
        }

        
    }
}
