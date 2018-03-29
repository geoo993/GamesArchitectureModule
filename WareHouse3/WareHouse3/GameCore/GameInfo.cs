using System;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WareHouse3
{
   
    public class GameInfo
    {
        public static readonly int ScreenWidth = (int)(double)GameManager.manager["ScreenWidth"];
        public static readonly int ScreenHeight = (int)(double)GameManager.manager["ScreenHeight"];
        public static readonly int MapWidth = (int)(double)GameManager.manager["MapWidth"];
        public static readonly int MapHeight = (int)(double)GameManager.manager["MapHeight"];
        public static readonly int LevelHorizontalLength = (int)(double)GameManager.manager["LevelHorizontalLength"];
        public static readonly int LevelVerticalLength = (int)(double)GameManager.manager["LevelVerticalLength"];
    
        public static readonly Camera Camera = new Camera();  
        public static readonly Random Random = new Random(DateTime.Now.Millisecond);
        public static GameStates GameStates { get; set; }
        
        private static GameInfo mInstance = null;
        public static GameInfo Instance
        {
              get
              {
                   if (mInstance == null)
                   {
                        mInstance = new GameInfo();
                   }
                 return mInstance;
              }
             
              set { mInstance = value; }
        }
        
        public Color RandomColor() {
            byte red = (byte)Random.Next(0, 255);
            byte green = (byte)Random.Next(0, 255);
            byte blue = (byte)Random.Next(0, 255);

            return new Color(red, green, blue);
        }
        
    }
    
    public class BallInfo {
        public static int BallSpeed = (int)(double)GameManager.manager["BallSpeed"];
        public static int BallJumpSpeed = (int)(double)GameManager.manager["BallJumpSpeed"];
    }
    
    public class TileInfo {
        public static int UnitWidth = (int)(double)GameManager.manager["TileWidth"];
        public static int UnitHeight = (int)(double)GameManager.manager["TileHeight"];
    }

    public class Device {
        public static GraphicsDevice graphicsDevice;
    }
    
    public class Commands {
        public static readonly CommandManager manager = new CommandManager();
    }
    
    public class GameStates
    {
    
        public enum GameState
        {
            Splash,
            Menu,
            Game, 
            Credits,
        }

        public GameStates()
        {
        }
    }
    
}
