using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace XylophoneGame
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
        
        
        public static float Value(float percent, float maxValue, float minValue) {
            float max = (maxValue > minValue) ? maxValue : minValue;
            float min = (maxValue > minValue) ? minValue : maxValue;
            return (((max - min) * percent) / 100.0f) + min;
        }
        
        public static float Percentage(float value, float maxValue, float minValue) {
            float difference = (minValue < 0) ? maxValue : maxValue - minValue;
            return ((value - minValue) / difference) * 100.0f;
        }
       
        
    }
}
