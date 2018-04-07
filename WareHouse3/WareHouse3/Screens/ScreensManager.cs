using System;
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
        
        public ContentManager ContentManager { get; private set; }
        public CommandManager CommandManager { get; private set; }
        public GameServiceContainer Services { get; private set; }
        
        public Screen CurrentScreen;
        public Rectangle ScreenSafeArea { get; private set; } 
      
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
            ScreenSafeArea = Rectangle.Empty;
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
            manager.AddKeyboardBinding(Keys.U, ProgressUp);
            manager.AddKeyboardBinding(Keys.I, ProgressDown);
            manager.AddKeyboardBinding(Keys.O, AutoPlaySwitch);
            
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
                var nextScreen = State + 1;
                SetState(nextScreen);
                //Debug.Print("Next Screen "+ nextScreen.ToString());
            }
        }
        
        public void PreviousScreen(ButtonAction buttonState, Vector2 amount)
        {
            
            if (buttonState == ButtonAction.PRESSED)
            {
                var previousScreen = State - 1;
                SetState(previousScreen);
				//Debug.Print("Previous Screen "+previousScreen.ToString());
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
        
        public void ProgressUp(ButtonAction buttonState, Vector2 amount)
        {
           
            if (CurrentScreen is LevelScreen && buttonState == ButtonAction.PRESSED)
            {
                //((LevelScreen)CurrentScreen).Hud.ProgressAmount += 1.0f;
                ((LevelScreen)CurrentScreen).SongProgressSpeed += 1.0f;
            }
        }
        
        public void ProgressDown(ButtonAction buttonState, Vector2 amount)
        {
            if (CurrentScreen is LevelScreen && buttonState == ButtonAction.PRESSED)
            {
                //((LevelScreen)CurrentScreen).Hud.ProgressAmount -= 1.0f;
                ((LevelScreen)CurrentScreen).SongProgressSpeed -= 1.0f;
            }
        }
        
        public void AutoPlaySwitch(ButtonAction buttonState, Vector2 amount)
        {
            if (CurrentScreen is LevelScreen && buttonState == ButtonAction.PRESSED)
            {
                ((LevelScreen)CurrentScreen).AutoPlay = !((LevelScreen)CurrentScreen).AutoPlay;
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
      

          //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Update(GameTime gameTime, Rectangle screenSafeArea)
        {
            
			var currentGameTime = gameTime.TotalGameTime;
			ScreenSafeArea = screenSafeArea;
            
            if (FSM != null)
            {
                FSM.Update(gameTime);
            }
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            if (CurrentScreen != null)
            {
                Vector2 hudLocation = new Vector2(GameInfo.Camera.Position.X - (ScreenSafeArea.Width / 2.0f), GameInfo.Camera.Position.Y - (ScreenSafeArea.Height / 2.0f) );
                var currentGameTime = gameTime.TotalGameTime;
                FSM.Draw(spriteBatch, hudLocation);
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
            
            Services = null;
        
            if (FSM != null)
            {
                FSM.Destroy();
                FSM = null;
            }
        }
        
        
    }
}
