using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// https://www.pinterest.co.uk/pin/614671049115390417/

namespace XylophoneGame
{

    public enum SongType {
        TwinkleLittle,
        JingleBells,
        RainRainGoAway,
        IncyIncySpider
    }
    
    public class XylophoneSongs {
    
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

        public static readonly List<SongType> Songs = new List<SongType>(){
            SongType.TwinkleLittle,
            SongType.JingleBells,
            SongType.RainRainGoAway,
            SongType.IncyIncySpider
        };
        
        public string GetSong(SongType type) {
            switch (type) {
            case SongType.JingleBells:
                    return "EEE EEE EGCD E F F FFF EE EG G FD C";
            case SongType.TwinkleLittle:
                    return "CCGGAAG FFEEDDC GGFFEED GGFFEED CCGGAAG FFEEDDC";
            case SongType.RainRainGoAway:
                    return "GEGGEGGE AGGE FFDDFFD GFEDECC";
            case SongType.IncyIncySpider:
                    return "CCC DEE E DCD EC E E FG GF EFGE CC DEE D CD EC C CCC DE E E DCD EC";
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
        
        
    }
    
}
