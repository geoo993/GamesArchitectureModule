using System;
using System.IO;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// https://github.com/andrekishimoto/fsm/tree/master/src/AK_Game/Gui/Screen

namespace WareHouse3
{
    public class ScreenManager
    {
        private FrameCounter frameCounter;
        
        protected ContentManager ContentManager;
        protected CommandManager CommandManager;
        protected GameServiceContainer Services;
        
        public Screen CurrentScreen { get; private set; }

        protected SplashScreen SplashScreen { get; private set; }
        protected MainScreen MainScreen;
        protected LevelScreen LevelScreen;
        protected WinScreen WinScreen;
        protected LoseScreen LoseScreen;
        
        /// <summary>
        /// HUD
        /// </summary>
        private SpriteFont HudFont;
        Progressbar HudProgressBar = null;
        
        /// <summary>
        /// Level.
        /// </summary>
        private Level Level;
        
        public bool ShouldExit { get; private set; }
        
        
        /// <summary>
        /// Screens state machine.
        /// </summary>
        private StateMachine FSM { get; set; }
        public ScreensState State { get; private set; }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public ScreenManager(ContentManager contentManager, CommandManager manager, GameServiceContainer service)
        {
            ContentManager = contentManager;
            CommandManager = manager;
            Services = service;
            CurrentScreen = null;
            HudProgressBar = new Progressbar();
            frameCounter = new FrameCounter();
        }
        
        private Texture2D CreateTexture(int Width, int Height, Color color) {
        
            Texture2D rectangle = new Texture2D(Device.graphicsDevice, Width, Height, false, SurfaceFormat.Color);
            
            Color[] colorData = new Color[Width * Height];
            for (int i = 0; i < Width * Height; i++)
                colorData[i] = color;
            rectangle.SetData<Color>(colorData);
            return rectangle;
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public virtual void Construct(Vector2 screenSize)
        {
            SplashScreen = new SplashScreen(ScreensType.SPLASH, this, ContentManager, GameInfo.SplashScreenCountDown);
            SplashScreen.Construct(Color.Cornsilk, ContentManager.Load<Texture2D>("SplashScreen"));

            MainScreen = new MainScreen(ScreensType.MAIN, this, ContentManager);
            MainScreen.Construct(Color.Black, ContentManager.Load<Texture2D>("MainMenu"));

            LevelScreen = new LevelScreen(ScreensType.LEVEL, this, ContentManager);
            LevelScreen.Construct(GameInfo.Instance.RandomColor(), CreateTexture((int)screenSize.X, (int)screenSize.Y, Color.Ivory));

            WinScreen = new WinScreen(ScreensType.WIN, this, ContentManager);
            WinScreen.Construct(Color.Black, ContentManager.Load<Texture2D>("GameWonImage"));
            
            LoseScreen = new LoseScreen(ScreensType.LOSE, this, ContentManager);
            LoseScreen.Construct(Color.Black, ContentManager.Load<Texture2D>("GameOverImage"));
            
            FSM = new StateMachine(this, 0.0f, null);
            SetState(ScreensState.SPLASH);
            HudFont = ContentManager.Load<SpriteFont>("Fonts/GameFont");

            SetKeyoardBindings(CommandManager);
            
            //LoadLevel("level3");
        }
        
        private void LoadLevel(string level)
        {
            // Unloads the content for the current level before loading the next one.
            if (Level != null)
                Level.Dispose();

            // Load the level.
            string levelPath = string.Format("Content/Levels/"+level+".txt");
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                Level = new Level(Services, fileStream, CommandManager);
        }
       
        public void SetKeyoardBindings(CommandManager manager) 
        {
            manager.AddKeyboardBinding(Keys.Escape, Exit);
            manager.AddKeyboardBinding(Keys.Enter, NextScreen);
            manager.AddKeyboardBinding(Keys.Back, PreviousScreen);
        }
        
        void Exit(ButtonAction buttonState, Vector2 amount)
        {
            
            if (buttonState == ButtonAction.PRESSED)
            {
                SetShouldExit(true);
            }
        }
        
        
        void NextScreen(ButtonAction buttonState, Vector2 amount)
        {
            
            if (buttonState == ButtonAction.PRESSED)
            {
                var nextScreen = State + 1;
                SetState(nextScreen);
                Debug.Print("Next Screen "+ nextScreen.ToString());
            }
        }
        
        void PreviousScreen(ButtonAction buttonState, Vector2 amount)
        {
            
            if (buttonState == ButtonAction.PRESSED)
            {
                var previousScreen = State - 1;
                SetState(previousScreen);
				Debug.Print("Previous Screen "+previousScreen.ToString());
            }
            
        }
        
       
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetShouldExit(bool shouldExit)
        {
            ShouldExit = shouldExit;
        }
        
        //-----------------------------------------------------------------------------
        //Set a New state of Finite State Machine
        //-----------------------------------------------------------------------------
        public void SetState(ScreensState state)
        {
            switch (state)
            {
                case ScreensState.SPLASH:
                    FSM.SetState(SplashScreenState.Instance());
                    break;
                case ScreensState.MAIN:
                    FSM.SetState(MainScreenState.Instance());
                    break;
                case ScreensState.LEVEL:
                    FSM.SetState(LevelScreenState.Instance());
                    break;
                case ScreensState.WIN:
                    FSM.SetState(WinScreenState.Instance());
                    break;
                case ScreensState.LOSE:
                    FSM.SetState(LoseScreenState.Instance());
                    break;
            }
            State = state;
            SetCurrent(State);
        }
        
        
        //-----------------------------------------------------------------------------
        // Set Current Screen
        //-----------------------------------------------------------------------------
        public virtual void SetCurrent(ScreensState state)
        {
            switch (state)
            {
                case ScreensState.SPLASH:
                    CurrentScreen = SplashScreen;
                    break;

                case ScreensState.MAIN:
                    CurrentScreen = MainScreen;
                    break;

                case ScreensState.LEVEL:
                    CurrentScreen = LevelScreen;
                    break;

                case ScreensState.WIN:
                    CurrentScreen = WinScreen;
                    break;
                case ScreensState.LOSE:
                    CurrentScreen = LoseScreen;
                    break;
            }
            
            CurrentScreen.OnEnter();
        }

          //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Update(GameTime gameTime)
        {
            
			var currentGameTime = gameTime.TotalGameTime;
            
            //Level.Update(gameTime, new Vector2(GameInfo.MapWidth, GameInfo.MapHeight));
            //HudProgressBar.Update(currentGameTime);
            
            
            frameCounter.Update(gameTime);
    
            if (FSM != null)
            {
                FSM.Update(currentGameTime);
            }

            if (CurrentScreen != null)
                CurrentScreen.Update(currentGameTime);
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Draw(GameTime gameTime, Rectangle titleSafeArea, SpriteBatch spriteBatch)
        {
            if (CurrentScreen != null)
            {
                Vector2 hudLocation = new Vector2(GameInfo.Camera.Position.X - (titleSafeArea.Width / 2.0f), GameInfo.Camera.Position.Y - (titleSafeArea.Height / 2.0f) );
                
                var currentGameTime = gameTime.TotalGameTime;
                CurrentScreen.Draw(currentGameTime, spriteBatch, hudLocation);
            
                //DrawHud(gameTime, titleSafeArea, spriteBatch);
                //Level.Draw(gameTime,  spriteBatch);
            }


        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Destroy()
        {
            if (SplashScreen != null)
            {
                SplashScreen.Destroy();
                SplashScreen = null;
            }

			if (MainScreen != null)
			{
				MainScreen.Destroy();
				MainScreen = null;
			}
			
            if (LevelScreen != null)
            {
                LevelScreen.Destroy();
                LevelScreen = null;
            }

            if (WinScreen != null)
            {
                WinScreen.Destroy();
                WinScreen = null;
            }
            
            if (LoseScreen != null)
            {
                LoseScreen.Destroy();
                LoseScreen = null;
            }

            CurrentScreen = null;

            if (FSM != null)
            {
                FSM.Destroy();
                FSM = null;
            }
        }
        
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
         private void DrawHud(GameTime gameTime, Rectangle titleSafeArea, SpriteBatch spriteBatch)
        {
            
            Vector2 hudLocation = new Vector2(GameInfo.Camera.Position.X, GameInfo.Camera.Position.Y - (titleSafeArea.Height / 2.0f) + 10);
            Color hudColor = Color.Yellow;

            // Draw time remaining. Uses modulo division to cause blinking when the
            // player is running out of time.
            string songName = XylophoneSongs.Instance.GetSong(Level.CurrentSong);
            Color hudBorderColor = Color.Yellow;
            
            float songNameWidth = HudFont.MeasureString(songName).X;
            float songNameHeight = HudFont.MeasureString(songName).Y;
            DrawShadowedString(spriteBatch, HudFont, songName, hudLocation + new Vector2(-(songNameWidth * 0.5f), 0.0f), hudColor, hudBorderColor);

            // Draw score
            DrawShadowedString(spriteBatch, HudFont, "SCORE: ", hudLocation + new Vector2(0.0f, songNameHeight * 1.2f), hudColor, hudBorderColor);
            
            
            HudProgressBar.Construct((int)songNameWidth, (int)songNameHeight, hudLocation - new Vector2(-(songNameWidth * 0.5f), 0.0f), Color.Orange, Color.SlateBlue);
            HudProgressBar.Draw(spriteBatch);
            
            //var fps = string.Format("FPS: {0}", frameCounter.AverageFramesPerSecond);
            //var fps2 = string.Format("Smooth FPS: {0}", smoothFPS.Framerate);
    
            //DrawShadowedString(hudFont, fps, new Vector2(hudLocation.X + 100 - (titleSafeArea.Width / 2.0f), hudLocation.Y), hudColor, hudColor);
            //DrawShadowedString(hudFont, fps, new Vector2(hudLocation.X + 100 - (titleSafeArea.Width / 2.0f), hudLocation.Y + (timeHeight * 1.2f)), hudColor, hudColor);
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color, Color borderColor)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), borderColor);
            spriteBatch.DrawString(font, value, position, color);
        }
    }
}
