using System;

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
        
    }
}
