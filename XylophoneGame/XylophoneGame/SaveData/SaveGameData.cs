using System.Collections.Generic;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
// https://stackoverflow.com/questions/24391890/saving-loading-data-on-level-selection-screen-xml-xna?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
// https://stackoverflow.com/questions/6201529/how-do-i-turn-a-c-sharp-object-into-a-json-string-in-net

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
        private static List<GameData> Levels = new List<GameData>();
        public static List<GameData> GameSave {
            get {
                return Levels;
            }
        }
        
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
        
        public static List<GameData> GetHighestScorers(int topScoresRequired) {

            if (Levels.Count < topScoresRequired)
                return Levels;
            
            var sortedLevels = Levels.OrderBy(i => i.Score).Reverse().ToArray();
            var biggestNumbers = new List<GameData>();
            
            /// https://stackoverflow.com/questions/620534/sort-array-of-items-using-orderby
            var biggestNumberCount = 1;
            var latestBiggest = sortedLevels.First();
            for (int i = 0; i < sortedLevels.Length; i++){
                var levelInfo = sortedLevels[i];
                if (biggestNumberCount >= topScoresRequired)
                    break;
                
                if (levelInfo.Score < latestBiggest.Score) {
                    latestBiggest = levelInfo;
                    biggestNumberCount += 1;
                }
                biggestNumbers.Add(levelInfo);
            }
            return biggestNumbers;
        }
    }
    
    
}
