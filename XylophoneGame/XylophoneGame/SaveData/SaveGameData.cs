using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using NLua;

    
namespace XylophoneGame
{
    
    public class SaveGameData
    {

        private string PlayerName;
        private string Level;
        private int Score;
        public Lua ctx = new Lua();
        
        public SaveGameData(string name, string level){
            PlayerName = name;
            Level = level;
            Score = 0;
        }
        
        public void DoAString() {
            
            double val = 12.0;
            GameManager.manager ["Abdul"] = val; // Create a global value 'x' 
            var res = GameManager.manager.DoString ("return 10 + Abdul*(5 + 2)")[0] as double?;
            Debug.Print(res.ToString());
        }
        
        
        public void Start()
        {
            ctx.RegisterFunction("Test", this, typeof(SaveGameData).GetMethod("Test"));
            ctx.LoadCLRPackage();
            ctx.DoFile("../Resources/Content/Scripts/test.lua");
        }

        public void Test(string str)
        {
            var expected = "abcd 가나다라 あかさた";
            Console.WriteLine("{0} (From C#)", expected); // for comparison purpose
            Console.WriteLine("{0} (From NLua)", str);
        }
        
    }
    
}
