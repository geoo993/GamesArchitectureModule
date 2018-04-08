using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WareHouse3
{
    
    //-----------------------------------------------------------------------------
    // ScreensState
    //
    // States of the screens
    //-----------------------------------------------------------------------------
    public enum ScreensState
    {
		SPLASH = 0,
		MAIN, 
		LEVEL,    
		WIN, 
		LOSE,
        
        COUNT  
    }
    
    public enum ScreensType
    {
        SPLASH = 0,
        MAIN, 
        LEVEL,    
        WIN, 
        LOSE  
    }

    //-----------------------------------------------------------------------------
    // SplashScreenState
    //
    // 
    //-----------------------------------------------------------------------------
    public class SplashScreenState : State
    {
        // Singleton
        private static SplashScreenState mInstance = null;
        public static SplashScreenState Instance()
        {
            if (mInstance == null)
                mInstance = new SplashScreenState();
            return mInstance;
        }

        SplashScreen screen;
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public SplashScreenState()
            : base()
        {
        
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Enter(ref Object owner)
        {
            
            ScreenManager manager = (ScreenManager)owner;
            screen = new SplashScreen(ScreensType.SPLASH, manager, manager.ContentManager, GameInfo.SplashScreenCountDown);
            screen.Construct(Color.Brown, manager.ContentManager.Load<Texture2D>("Splash2"));
			screen.OnEnter();
            manager.CurrentScreen = screen;
            
            Debug.Print("Enter: Splash Screen");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, GameTime gameTime)
        {
            //Debug.Print("Update: Splash Screen");
            screen.Update(gameTime);
        }
           
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------   
        public override void Draw(ref Object owner, SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
            screen.Draw(spriteBatch, screenSafeArea);
        }
         
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Debug.Print("Exit: Splash Screen");
            
            screen.OnExit();
        }
    }
    
    
    //-----------------------------------------------------------------------------
    // MainScreenState
    //
    // 
    //-----------------------------------------------------------------------------
    public class MainScreenState : State
    {
        // Singleton
        private static MainScreenState mInstance = null;
        public static MainScreenState Instance()
        {
            if (mInstance == null)
                mInstance = new MainScreenState();
            return mInstance;
        }

        MainScreen screen;
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public MainScreenState()
            : base()
        {
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Enter(ref Object owner)
        {
        
            ScreenManager manager = (ScreenManager)owner;
            screen = new MainScreen(ScreensType.MAIN, manager, manager.ContentManager);
            screen.Construct(Color.Black, manager.ContentManager.Load<Texture2D>("MainMenu"));
            screen.OnEnter();
            manager.CurrentScreen = screen;
            Debug.Print("Enter: Main Screen");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, GameTime gameTime)
        {
            screen.Update(gameTime);
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------   
        public override void Draw(ref Object owner, SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
            screen.Draw(spriteBatch, screenSafeArea);
        }
       
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Debug.Print("Exit: Main Screen");
            screen.OnExit();
        }
    }
    
    
    //-----------------------------------------------------------------------------
    // LevelScreenState
    //
    // 
    //-----------------------------------------------------------------------------
    public class LevelScreenState : State
    {
        // Singleton
        private static LevelScreenState mInstance = null;
        public static LevelScreenState Instance()
        {
            if (mInstance == null)
                mInstance = new LevelScreenState();
            return mInstance;
        }

        LevelScreen screen; 

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public LevelScreenState()
            : base()
        {
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Enter(ref Object owner)
        {
           
            ScreenManager manager = (ScreenManager)owner;
            screen = new LevelScreen(ScreensType.LEVEL, manager, manager.ContentManager);
            screen.Construct(GameInfo.Instance.RandomColor(), null);
            manager.CurrentScreen = screen;
			screen.OnEnter();
            
            Debug.Print("Enter: Level Screen");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, GameTime gameTime)
        {
            //Debug.Print("Update: Level Screen");
            screen.Update(gameTime);
        }  
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------   
        public override void Draw(ref Object owner, SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
            screen.Draw(spriteBatch, screenSafeArea);
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Debug.Print("Exit: Level Screen");
            screen.OnExit();
        }
    }
    
    
    //-----------------------------------------------------------------------------
    // WinScreenState
    //
    // 
    //-----------------------------------------------------------------------------
    public class WinScreenState : State
    {
        // Singleton
        private static WinScreenState mInstance = null;
        public static WinScreenState Instance()
        {
            if (mInstance == null)
                mInstance = new WinScreenState();
            return mInstance;
        }

        WinScreen screen;

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public WinScreenState()
            : base()
        {
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Enter(ref Object owner)
        {
            ScreenManager manager = (ScreenManager)owner;
            screen = new WinScreen(ScreensType.WIN, manager, manager.ContentManager);
            screen.Construct(Color.Black, manager.ContentManager.Load<Texture2D>("GameWonImage"));
            screen.OnEnter();
            manager.CurrentScreen = screen;
                 
            Debug.Print("Enter: Win Screen");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, GameTime gameTime)
        {
            screen.Update(gameTime);
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------   
        public override void Draw(ref Object owner, SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
            screen.Draw(spriteBatch, screenSafeArea);
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Debug.Print("Exit: Win Screen");
            screen.OnExit();
        }
    }
    
    
    
    //-----------------------------------------------------------------------------
    // LoseScreenState
    //
    // 
    //-----------------------------------------------------------------------------
    public class LoseScreenState : State
    {
        // Singleton
        private static LoseScreenState mInstance = null;
        public static LoseScreenState Instance()
        {
            if (mInstance == null)
                mInstance = new LoseScreenState();
            return mInstance;
        }

        LoseScreen screen; 

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public LoseScreenState()
            : base()
        {
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Enter(ref Object owner)
        {
            ScreenManager manager = (ScreenManager)owner;
            screen = new LoseScreen(ScreensType.LOSE, manager, manager.ContentManager);
            screen.Construct(Color.Black, manager.ContentManager.Load<Texture2D>("GameOverImage"));
            screen.OnEnter();
            manager.CurrentScreen = screen;
                    
            Debug.Print("Enter: Lose Screen");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, GameTime gameTime)
        {
            screen.Update(gameTime);
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------   
        public override void Draw(ref Object owner, SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
            screen.Draw(spriteBatch, screenSafeArea);
        }
       
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Debug.Print("Exit: Lose Screen");
            screen.OnExit();
        }
    }
    
}
