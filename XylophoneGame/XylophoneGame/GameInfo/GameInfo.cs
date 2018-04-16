using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;
using NLua;

namespace XylophoneGame
{

    public class GameInfo
    {
        public static readonly int ScreenWidth = (int)(double)LuaInfo.manager["ScreenWidth"];
        public static readonly int ScreenHeight = (int)(double)LuaInfo.manager["ScreenHeight"];
        public static readonly int MapWidth = (int)(double)LuaInfo.manager["MapWidth"];
        public static readonly int MapHeight = (int)(double)LuaInfo.manager["MapHeight"];
        public static readonly int LevelHorizontalLength = (int)(double)LuaInfo.manager["LevelHorizontalLength"];
        public static readonly int LevelVerticalLength = (int)(double)LuaInfo.manager["LevelVerticalLength"];
        public static readonly int SplashScreenCountDown = (int)(double)LuaInfo.manager["SplashScreenCountDown"];
        public static readonly string Player = (string)LuaInfo.manager["PlayerName"];
        public static readonly Random Random = new Random(DateTime.Now.Millisecond);

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

        public Color RandomColor()
        {
            byte red = (byte)Random.Next(0, 255);
            byte green = (byte)Random.Next(0, 255);
            byte blue = (byte)Random.Next(0, 255);
            return new Color(red, green, blue);
        }
    }

    public class LevelInfo
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);
        private static readonly List<string> Levels = new List<string>() { "level1", "level2", "level3" };
        public static string RandomLevel
        {
            get { return Levels[Random.Next(Levels.Count)]; }
        }
        
        public static string LevelAt(int index)
        {
           return Levels[index];
        }
        
    }
    
    public class BallInfo {
        public static int BallSpeed = (int)(double)LuaInfo.manager["BallSpeed"];
        public static int BallJumpSpeed = (int)(double)LuaInfo.manager["BallJumpSpeed"];
		public static float BallMass = (int)(double)LuaInfo.manager["BallMass"];
    }
    
    public class TileInfo {
        public static int UnitWidth = (int)(double)LuaInfo.manager["TileWidth"];
        public static int UnitHeight = (int)(double)LuaInfo.manager["TileHeight"];
        public static int MoveSpeed = (int)(double)LuaInfo.manager["TileMoveSpeed"];
    }
    
    public class CollectableInfo {
        public static int Radius = (int)(double)LuaInfo.manager["CollectableRadius"];
    }
 
    public class LuaInfo
    {
        private static Lua _manager = new Lua();
        public static Lua manager
        {
            get
            {
                var folderName = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                // Our executable is inside /MonoBundle
                // So we need to go back one folder and enter "Resources/Content/Scripts/player.lua
                 _manager.DoFile("../Resources/Content/Scripts/GameManager.lua");
                //_manager = state.DoString("return 200")[0];
            
                return _manager;
            }
        }
    }
    
}
