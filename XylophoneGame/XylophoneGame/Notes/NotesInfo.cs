using System;
using System.Collections.Generic;


namespace XylophoneGame
{
  
    public class NoteInfo {
    
        public static readonly Dictionary<string, string> AvailableNotes = new Dictionary<string, string>()
        {
            {"NoteA", "note6"},
            {"NoteB", "note7"},
            {"NoteC", "note1"},
            {"NoteC2", "note8"},
            {"NoteD", "note2"},
            {"NoteE", "note3"},
            {"NoteF", "note4"},
            {"NoteG", "note5"},
        };
        
        // https://stackoverflow.com/questions/2444033/get-dictionary-key-by-value
        public static string KeyByValue(string value)
        {
            string key = null;
            foreach (KeyValuePair<string, string> pair in AvailableNotes)
            {
                if (pair.Value == value)
                { 
                    key = pair.Key; 
                    break; 
                }
            }
            return key;
        }
        
        // https://stackoverflow.com/questions/1028136/random-entry-from-dictionary
        public static TKey[] Shuffle<TKey, TValue>(Dictionary<TKey, TValue> source)
        {
            Random r = new Random();
            TKey[] wviTKey = new TKey[source.Count];
            source.Keys.CopyTo(wviTKey, 0);
    
            for (int i = wviTKey.Length; i > 1; i--)
            {
                int k = r.Next(i);
                TKey temp = wviTKey[k];
                wviTKey[k] = wviTKey[i - 1];
                wviTKey[i - 1] = temp;
            }
    
            return wviTKey;
        }
        
        
        public static TValue UniqueRandomValue<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
            Dictionary<TKey, TValue> values = new Dictionary<TKey, TValue>(dict);
            
            List<TKey> keyList = new List<TKey>(values.Keys);
            TKey randomKey = keyList[GameInfo.Random.Next(keyList.Count)];
            TValue randomValue = values[randomKey];
            return randomValue;
        }

    }

}
