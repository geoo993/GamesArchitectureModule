using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// https://github.com/andrekishimoto/fsm/tree/master/src/AK_Game/Gui/Screen

namespace XylophoneGame
{
    public class ScreenManager
    {
        public ScoreSubject ScoreSubject { get; private set; }
        
        
        public string Title { get; private set; }
        public string Player { get; private set; }
        public string Level { get; private set; }
        public SongType Song { get; private set; }
        public float SongSpeed { get; private set; }

        public ContentManager ContentManager { get; private set; }
        public CommandManager CommandManager { get; private set; }
        public GameServiceContainer Services { get; private set; }

        public Screen CurrentScreen;

        public bool ShouldExit { get; private set; }

        /// <summary>
        /// Screens state machine.
        /// </summary>
        private StateMachine FSM { get; set; }
        public ScreensState State { get; private set; }
        
        /// <summary>
        /// delay time to wait 8 seconds before setting GameEnd method
        /// </summary>
        private static readonly TimeSpan GameEndInterval = TimeSpan.FromMilliseconds(8000);
        private TimeSpan GameEndLastTimeInterval;

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public ScreenManager(string title, string player, ContentManager contentManager, CommandManager manager, GameServiceContainer service)
        {
            Title = title;
            Player = player;
            Level = LevelInfo.RandomLevel;
            Song = XylophoneSongs.RandomSong;
            SongSpeed = XylophoneSongs.Songs[Song];
            ContentManager = contentManager;
            CommandManager = manager;
            Services = service;
            CurrentScreen = null;
            
            ScoreSubject = new ScoreSubject();
            ScoreSubject.StartScoreSystem(XylophoneSongs.Instance.GetSong(Song), SongSpeed);
            
        }

        public void Reset()
        {
            Level = LevelInfo.RandomLevel;
            Song = XylophoneSongs.RandomSong;
            SongSpeed = XylophoneSongs.Songs[Song];

            if (ScoreSubject != null)
            {
                ScoreSubject.EndScoreSystems();
                ScoreSubject = null;
            }
			ScoreSubject = new ScoreSubject();
			ScoreSubject.StartScoreSystem(XylophoneSongs.Instance.GetSong(Song), SongSpeed);
            
            FSM.ClearCurrentState();
            SetState(ScreensState.LEVEL);
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public virtual void Construct()
        {
            FSM = new StateMachine(this, 0.0f, null);
            SetState(ScreensState.SPLASH);
            SetKeyoardBindings(CommandManager);
        }

        #region KeyBoard Actions
        public void SetKeyoardBindings(CommandManager manager)
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
                ((LevelScreen)CurrentScreen).Level.Ball.SetUpMovement(buttonState);
            }
        }

        public void DownArrow(ButtonAction buttonState, Vector2 amount)
        {
            if (CurrentScreen is LevelScreen)
            {
                ((LevelScreen)CurrentScreen).Level.Ball.SetDownMovement(buttonState);
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
        
            if (FSM != null)
            {
                FSM.Destroy();
                FSM = null;
            }
        }
        
        
    }
}
