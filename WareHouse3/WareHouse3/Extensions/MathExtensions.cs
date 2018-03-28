using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace WareHouse3
{
    public class MathExtensions
    {
        public static float pi = 3.14159265359f;
        
        public static float DegreeToRadians(float degree) {
            return degree * pi / 180.0f; 
        }
        public static float RadianToDegrees(float radian) {
            return radian * 180.0f / pi; 
        }
        
        public static float GetDistance(Vector2 start, Vector2 end)
        {
           
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            
            float distance = (float)Math.Sqrt( dx * dx + dy * dy);
            return distance;
        }
        
    }
}
