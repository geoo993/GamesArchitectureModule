using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// https://www.pinterest.co.uk/pin/614671049115390417/

namespace XylophoneGame
{

    public enum SongType {
        TwinkleLittle,
        JingleBells,
        IncyIncySpider
    }
    
    public class XylophoneSongs {
    
		private static readonly Random Random = new Random(DateTime.Now.Millisecond);
        
        private static XylophoneSongs instance = null;
        public static XylophoneSongs Instance
        {
            get
            {
                if (instance == null)
                    instance = new XylophoneSongs();
                return instance;
            }
        }

        public static readonly Dictionary<SongType, float> Songs = new Dictionary<SongType, float>() {
            {SongType.TwinkleLittle, (float)(double)GameManager.manager["TwinkleLittleSpeed"]},
            {SongType.JingleBells, (float)(double)GameManager.manager["JingleBellsSpeed"]},
            {SongType.IncyIncySpider, (float)(double)GameManager.manager["IncyIncySpiderSpeed"]}
        };
        
         
        public static SongType RandomSong {
            get
            {
                return Songs.Keys.ToList()[Random.Next(Songs.Count)];
            }
        }
        
        public static SongType SongAt(int index) {
            return Songs.Keys.ToList()[index];
        }
        
        public string GetSong(SongType type) {
            switch (type) {
            case SongType.JingleBells:
                    return (string)GameManager.manager["JingleBells"];
            case SongType.TwinkleLittle:
                    return (string)GameManager.manager["TwinkleLittle"];
            case SongType.IncyIncySpider:
                    return (string)GameManager.manager["IncyIncySpider"];
            default:
                    return "";
            }
        }
        
        public string GetNoteName(char note) {
            switch (note) {
            case 'A':
                    return "NoteA";
            case 'B':
                    return "NoteB";
            case 'C':
                    return "NoteC";
            case 'D':
                    return "NoteD";
            case 'E':
                    return "NoteE";
            case 'F':
                    return "NoteF";
            case 'G':
                    return "NoteG";
            default:
                    return null;
            }
        }
        
        public int GetIndexOfNextNote(SongType type, int currentIndex) {
            string song = GetSong(type);
            int index = 0;
            for (index = currentIndex; index < song.Length; ++index) {
                var tempLetter = song[index];
                if (tempLetter != ' ') {
                    break;
                }
            }
            return index;
        }
        
        public int GetNumberOfNotesInSong(string song) {
            int count = 0;
            for (int i = 0; i < song.Length; ++i) {
                var tempLetter = song[i];
                if (tempLetter != ' ') {
                    count++;
                }
            }
            return count;
        }
        
        
    }
    
}
