#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;

using AppKit;
using Foundation;
#endregion

namespace ChaseCameraGameLab3.MacOS
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            NSApplication.Init();

            using (ChaseCameraGame game = new ChaseCameraGame())
            {
                game.Run();
            }
        }
    }
    
}
