using System;
using System.Reflection;
using NLua;

namespace WareHouse3
{

    public class GameManager
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
        
        public GameManager()
        {
        
        }
    }
   
}
