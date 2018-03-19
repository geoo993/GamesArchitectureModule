using System;
using Microsoft.Xna.Framework;

namespace WareHouse3
{
    public class EnemyInfo
    {
        public static readonly float Speed = 100.0f;
    }
    
    public class GemInfo
    {
        public static readonly float BounceHeight = 0.18f;
        public static readonly float BounceRate = 6.0f;
        public static readonly Color Color = Color.Red;
    }
    
    public class TileInfo {
        
        public const int Width = 40;
        public const int Height = 32;

        public static readonly Vector2 Size = new Vector2(Width, Height);

    }
     
    public class GameInfo
    {
        public static readonly int screenWidth = (int)(double)GameManager.manager["ScreenWidth"];
        public static readonly int screenHeight = (int)(double)GameManager.manager["ScreenHeight"];
    }


}
