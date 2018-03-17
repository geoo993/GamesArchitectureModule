#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AppKit;
using Foundation;
using NLua;
#endregion

namespace WareHouse3
{

    public static class GameManager
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
    
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            NSApplication.Init();

            using (var game = new Game1())
            {
                game.Run();
            }
        }
    }
}
