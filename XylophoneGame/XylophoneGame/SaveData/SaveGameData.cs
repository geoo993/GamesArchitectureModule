using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace XylophoneGame
{
   
    struct GameData
    {
        public string PlayerName;
        public string Level;
        public int Score;
        public bool DidWin;
        
        internal GameData(string name, string level, int score, bool didWin)
        {
            this.PlayerName = name;
            this.Level = level;
            this.Score = score;
            this.DidWin = didWin;
        }
    }


    class SaveLoadJSON
    {
        public static List<GameData> Levels = new List<GameData>();
        
        public static void Load()
        {
            using (StreamReader file = File.OpenText("../Resources/Content/Scripts/GameData.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                Levels = (List<GameData>)serializer.Deserialize(file, typeof(List<GameData>));
            }  
        }
    
        public static void Save(GameData level)
        {
            Levels.Add(level);
            if (File.Exists("../Resources/Content/Scripts/GameData.json"))
            {
                File.Delete("../Resources/Content/Scripts/GameData.json");
            }
    
            using (FileStream fs = File.Open("../Resources/Content/Scripts/GameData.json", FileMode.CreateNew))
            using (StreamWriter sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
    
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, Levels);
            }
        }
    }
    
    
}
