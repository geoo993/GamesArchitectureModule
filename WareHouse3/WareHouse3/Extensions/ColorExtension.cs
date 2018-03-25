using System;
using Microsoft.Xna.Framework;

namespace WareHouse3
{
    public class ColorExtension
    {
        public static Color Random {
            get {
            
                Random r = new Random ( );
                return new Color (
    
                    ( byte ) r.Next ( 0, 255 ),
    
                    ( byte ) r.Next ( 0, 255 ),
    
                    ( byte ) r.Next ( 0, 255 ) );
            }
        }
    }
}
