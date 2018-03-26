using System;
using System.Security.Cryptography;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WareHouse3
{
    public class ObstatclesInfo
    {
        public static readonly float MaxSpeed = (int)(double)GameManager.manager["ObstatclesMaxSpeed"];
        public static readonly int NumberOfObstacles = (int)(double)GameManager.manager["NumberOfObstacles"];
    }
    
    public class GameInfo
    {
        public static readonly int ScreenWidth = (int)(double)GameManager.manager["ScreenWidth"];
        public static readonly int ScreenHeight = (int)(double)GameManager.manager["ScreenHeight"];
        public static readonly int MapWidth = (int)(double)GameManager.manager["MapWidth"];
        public static readonly int MapHeight = (int)(double)GameManager.manager["MapHeight"];
        public static readonly Camera Camera = new Camera();  
        public static readonly Random Random = new Random(DateTime.Now.Millisecond);
        public static GameStates GameStates { get; set; }
       
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
