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
        
         #endregion
        
        public Game1()
        {
            commandManager = new CommandManager();
            graphics = new GraphicsDeviceManager(this);
            
            //System.Diagnostics.Debug.Print(this.graphics.PreferredBackBufferHeight.ToString());
            //System.Diagnostics.Debug.Print(this.graphics.PreferredBackBufferWidth.ToString());
            
            float aspectRatio = (float)this.graphics.PreferredBackBufferWidth / this.graphics.PreferredBackBufferHeight;
            float size = (float)(double)GameManager.manager["ScreenSize"];
            screenSize = new Vector2((size * aspectRatio), size);
            graphics.PreferredBackBufferWidth = (int)screenSize.X;
            graphics.PreferredBackBufferHeight = (int)screenSize.Y;
            graphics.IsFullScreen = false;
            
            Content.RootDirectory = "Content";
            
            IsMouseVisible = true;
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
            
            backgroundSprite = this.spriteSheet.Sprite(TexturePackerMonoGameDefinitions.SpriteSheet.Background);
            centreScreen = new Vector2 (this.GraphicsDevice.Viewport.Width / 2, this.GraphicsDevice.Viewport.Height / 2);

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
            
            // TODO: Add your update logic here
            characterAnimationManager.Update(gameTime);
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
            
            // Draw the background
            this.spriteRender.Draw(this.backgroundSprite, this.centreScreen);
            
            // Draw character on screen
            
            this.spriteRender.Draw(
                this.spriteSheet.Sprite(
                    TexturePackerMonoGameDefinitions.SpriteSheet.Cowboy_Idle
                ),
                new Vector2(100, 400)
            );
            
            
            // Draw character on screen
            this.spriteRender.Draw(
                this.characterAnimationManager.CurrentSprite, 
                this.characterAnimationManager.CurrentPosition, 
                Color.White, 0, 1,
                this.characterAnimationManager.CurrentSpriteEffects);

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

            var animationWalkRight = new Animation(new Vector2(characterVelocityPixelsPerSecond, 0), this.timePerFrame, SpriteEffects.None, TexturePackerMonoGameDefinitions.SpriteSheet.capguyWalkSprites);
            var animationWalkLeft = new Animation(new Vector2(-characterVelocityPixelsPerSecond, 0), this.timePerFrame, SpriteEffects.FlipHorizontally, TexturePackerMonoGameDefinitions.SpriteSheet.capguyWalkSprites);
            var animationTurnRightToLeft = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.None, TexturePackerMonoGameDefinitions.SpriteSheet.capguyTurnSprites);
            var animationTurnLeftToRight = new Animation(Vector2.Zero, this.timePerFrame, SpriteEffects.FlipHorizontally, TexturePackerMonoGameDefinitions.SpriteSheet.capguyTurnSprites);

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
