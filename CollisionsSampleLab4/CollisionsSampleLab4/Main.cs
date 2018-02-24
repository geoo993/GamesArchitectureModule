#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;

using AppKit;
using Foundation;
#endregion

namespace CollisionsSampleLab4.MacOS
{
    #region Entry Point

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            NSApplication.Init();

            //using (var game = new Game1())
            //{
            //    game.Run();
            //}
            
            using (ChaseCameraGame game = new ChaseCameraGame())
            {
                game.Run();
            }
            
        }
    }
    #endregion
}