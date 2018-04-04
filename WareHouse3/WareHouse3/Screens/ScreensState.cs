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
           
            Debug.Print("Enter: Splash Screen");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, TimeSpan currentGameTime)
        {
            ScreenManager screen = (ScreenManager)owner;
            SplashScreen splash = (SplashScreen)screen.CurrentScreen;
            
            splash.CountDown -= currentGameTime.Seconds;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Debug.Print("Exit: Splash Screen");
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
           
            Debug.Print("Enter: Main Screen");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, TimeSpan currentGameTime)
        {
            
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Debug.Print("Exit: Main Screen");
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
           
            Debug.Print("Enter: Level Screen");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, TimeSpan currentGameTime)
        {
            
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Debug.Print("Exit: Level Screen");
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
           
            Debug.Print("Enter: Win Screen");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, TimeSpan currentGameTime)
        {
            
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Debug.Print("Exit: Win Screen");
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
           
            Debug.Print("Enter: Lose Screen");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(ref Object owner, TimeSpan currentGameTime)
        {
            
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Exit(ref Object owner)
        {
            Debug.Print("Exit: Lose Screen");
        }
    }
    
}
