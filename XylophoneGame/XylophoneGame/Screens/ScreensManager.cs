using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// https://github.com/andrekishimoto/fsm/tree/master/src/AK_Game/Gui/Screen

namespace XylophoneGame
{
    public class ScreenManager
    {
        /// <summary>
        /// Observable Subject
        /// </summary>
        public ScoreSubject ScoreSubject { get; private set; }
        
		/// <summary>
		/// Screens state machine.
		/// </summary>
		private StateMachine FSM { get; set; }
		public ScreensState State { get; private set; }
        
        /// <summary>
        /// Game info .
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; private set; }
        public string Player { get; private set; }
        public string Level { get; private set; }
        public SongType SongType { get; private set; }
        public string Song
        {
            get { return XylophoneSongs.Instance.GetSong(SongType);  }
        }
        public float SongSpeed { get; private set; }

        /// <summary>
        /// managers
        /// </summary>
        /// <value>The content manager.</value>
        public ContentManager ContentManager { get; private set; }
        public CommandManager CommandManager { get; private set; }
        public GameServiceContainer Services { get; private set; }
        public GraphicsDevice GraphicsDevice { get; private set; }
        public Camera Camera { get; private set; }
        
        /// <summary>
        /// Game screens.
        /// </summary>
        public Screen CurrentScreen;
        public Vector2 ScreenTopCenter { get; private set; }
        
        /// <summary>
        /// Game Fonts
        /// </summary>
        public SpriteFont HudFont { get; private set; }
        public SpriteFont HudLargeFont { get; private set; }
        public float HudFlashScoreCount { get; private set; }
        public float HudWidth {
            get {
                if (HudFont != null) {
                    return HudFont.MeasureString(Song).X;
                }
                return 0.0f;
            }
        }
        public float HudHeight {
            get {
                if (HudFont != null) {
                    return HudFont.MeasureString(Song).Y;
                }
                return 0.0f;
            }
        }
        
        public bool ShouldExit { get; private set; }

        
        /// <summary>
        /// delay time to wait 10 seconds before setting GameEnd method
        /// </summary>
        private static readonly TimeSpan GameEndInterval = TimeSpan.FromMilliseconds(10000);
        private TimeSpan GameEndLastTimeInterval;

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public ScreenManager(string title, string player, ContentManager contentManager, GraphicsDevice graphicsDevice, GameServiceContainer service)
        {
            Title = title;
            Player = player;
            Level = LevelInfo.RandomLevel;
            SongType = XylophoneSongs.RandomSong;
            SongSpeed = XylophoneSongs.Songs[SongType];
            ContentManager = contentManager;
            CommandManager = new CommandManager();
            GraphicsDevice = graphicsDevice;
            Camera = new Camera(graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            Services = service;
            CurrentScreen = null;
            ScreenTopCenter = Vector2.Zero;
            
            ScoreSubject = new ScoreSubject();
            ScoreSubject.StartScoreSystem(Song, SongSpeed);
            
			FSM = new StateMachine(this, 0.0f, null);
            HudFont = null;
            HudLargeFont = null;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public virtual void Construct()
        {
            HudFont = ContentManager.Load<SpriteFont>("Fonts/GameFont");
            HudLargeFont = ContentManager.Load<SpriteFont>("Fonts/LargeGameFont");
            
            SetState(ScreensState.SPLASH);
            SetKeyboardBindings(CommandManager);
            
        }

        #region KeyBoard Actions
        public void SetKeyboardBindings(CommandManager manager)
        {
            manager.AddKeyboardBinding(Keys.Escape, Exit);
            manager.AddKeyboardBinding(Keys.Enter, NextScreen);
            manager.AddKeyboardBinding(Keys.Back, PreviousScreen);
            manager.AddKeyboardBinding(Keys.Left, LeftArrow);
            manager.AddKeyboardBinding(Keys.Right, RightArrow);
            manager.AddKeyboardBinding(Keys.Up, UpArrow);
            manager.AddKeyboardBinding(Keys.Down, DownArrow);
            manager.AddKeyboardBinding(Keys.Space, Space);
            manager.AddKeyboardBinding(Keys.O, AutoPlaySwitch);
            manager.AddKeyboardBinding(Keys.R, RestartButton);

        }

        public void Exit(ButtonAction buttonState, Vector2 amount)
        {

            if (buttonState == ButtonAction.PRESSED)
            {
                SetShouldExit(true);
            }
        }


        public void NextScreen(ButtonAction buttonState, Vector2 amount)
        {

            if (buttonState == ButtonAction.PRESSED)
            {
                ScreensNavigation();
            }
        }

        public void PreviousScreen(ButtonAction buttonState, Vector2 amount)
        {

            if (buttonState == ButtonAction.PRESSED)
            {

            }
        }

        public void LeftArrow(ButtonAction buttonState, Vector2 amount)
        {
            if (CurrentScreen is LevelScreen)
            {
                ((LevelScreen)CurrentScreen).Level.Ball.SetLeftMovement(buttonState);
            }
        }

        public void RightArrow(ButtonAction buttonState, Vector2 amount)
        {
            if (CurrentScreen is LevelScreen)
            {
                ((LevelScreen)CurrentScreen).Level.Ball.SetRightMovement(buttonState);
            }
        }

        public void UpArrow(ButtonAction buttonState, Vector2 amount)
        {
            if (CurrentScreen is LevelScreen)
            {
                ((LevelScreen)CurrentScreen).Level.Ball.SetScaleUp(buttonState);
            }
        }

        public void DownArrow(ButtonAction buttonState, Vector2 amount)
        {
            if (CurrentScreen is LevelScreen)
            {
                ((LevelScreen)CurrentScreen).Level.Ball.SetScaleDown(buttonState);
            }
        }

        public void Space(ButtonAction buttonState, Vector2 amount)
        {
            if (CurrentScreen is LevelScreen)
            {
                ((LevelScreen)CurrentScreen).Level.Ball.SetJumpMovement(buttonState);
            }
        }

        public void AutoPlaySwitch(ButtonAction buttonState, Vector2 amount)
        {
            if (CurrentScreen is LevelScreen && buttonState == ButtonAction.PRESSED)
            {
                ((LevelScreen)CurrentScreen).Parent.ScoreSubject.SwitchAutopPlay();
            }
        }

        public void RestartButton(ButtonAction buttonState, Vector2 amount)
        {
            if (CurrentScreen is LevelScreen && buttonState == ButtonAction.PRESSED)
            {
                Reset();
            }
        }

        private void ScreensNavigation()
        {
            if (CurrentScreen is MainScreen)
            {
                Reset();
                SetState(ScreensState.LEVEL);
            }
            else if (CurrentScreen is WinScreen)
            {
                SetState(ScreensState.MAIN);
            }
            else if (CurrentScreen is LoseScreen)
            {
                SetState(ScreensState.MAIN);
            }

        }

        #endregion


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
        }


        private void SetGameEndScreen()
        {

            if (CurrentScreen is LevelScreen)
            {
                ScoreObserver observer = new ScoreObserver("Game Observer");
                observer.Subscribe(((LevelScreen)CurrentScreen).Parent.ScoreSubject);

                if (observer.HasSongEnded)
                {
                    if (observer.IsGameSuccess)
                    {
                        SetState(ScreensState.WIN);
                    }
                    else
                    {
                        SetState(ScreensState.LOSE);
                    }
                    //Debug.Print("Game Success " + observer.IsGameSuccess.ToString());
                }
            }
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Update(GameTime gameTime)
        {
            
            // Update the command manager (updates polling input and fires input events)
            CommandManager.Update();
            
            // short delay before we go to game end
            if (GameEndLastTimeInterval + GameEndInterval < gameTime.TotalGameTime)
            {
                SetGameEndScreen();
                GameEndLastTimeInterval = gameTime.TotalGameTime;
            }
            
            if (FSM != null)
            {
                FSM.Update(gameTime);
            }
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Draw(SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
            
            if (CurrentScreen != null)
            {
                ScreenTopCenter = new Vector2(Camera.Position.X, Camera.Position.Y - (screenSafeArea.Height / 2.0f));
                FSM.Draw(spriteBatch, screenSafeArea);
            }
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Destroy()
        {
            CurrentScreen.Destroy();
            CurrentScreen = null;

            ContentManager.Dispose();
            ContentManager = null;

            CommandManager.Destroy();
            CommandManager = null;
            
            ScoreSubject.EndScoreSystems();
            ScoreSubject = null;
            
            Services = null;
            
            HudFont = null;
            HudLargeFont = null;
        
            if (FSM != null)
            {
                FSM.Destroy();
                FSM = null;
            }
        }
        
        
        public void Reset()
        {
            Level = LevelInfo.RandomLevel;
            SongType = XylophoneSongs.RandomSong;
            SongSpeed = XylophoneSongs.Songs[SongType];

            if (ScoreSubject != null)
            {
                ScoreSubject.EndScoreSystems();
                ScoreSubject = null;
            }
            ScoreSubject = new ScoreSubject();
            ScoreSubject.StartScoreSystem(Song, SongSpeed);
            
            FSM.ClearCurrentState();
            SetState(ScreensState.LEVEL);
        }
        
        public void ShowResults(SpriteBatch spriteBatch, Rectangle screenSafeArea, string result, bool useLargeFont) {

            var hud = useLargeFont ? HudLargeFont : HudFont;
			HudFlashScoreCount = (HudFlashScoreCount + 0.02f) % 1.0f;
            var textColor = Color.Bisque;
            var position = ScreenTopCenter + new Vector2(-(screenSafeArea.Width * 0.5f) + (HudWidth * 0.3f), 0.0f);
            var largetFontWidth = hud.MeasureString(result).X;
            var color = textColor * HudFlashScoreCount;
            var origin = new Vector2(largetFontWidth / 2, 0);
            DrawShadowedString(spriteBatch, hud, result, position, origin, color, 1.0f);
            
            
            var highestScorers = SaveLoadJSON.GetHighestScorers(3);
            var highestScorersOffset = 50.0f;
            
            for (int i = 0; i < highestScorers.Count; i++)
            {
                var topScorer = highestScorers[i];
                var displayScorer = topScorer.PlayerName +" played " + topScorer.Score + " notes";
                DrawShadowedString(spriteBatch, hud, displayScorer, position + new Vector2(0.0f, highestScorersOffset), origin, color, 1.0f);
                highestScorersOffset += 50.0f;
            }

        }
        
        public static void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Vector2 origin, Color color, float scale)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), color, 0.0f, origin, scale, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, value, position, color, 0.0f, origin, scale, SpriteEffects.None, 0.0f);
        }
        
        
    }
}
