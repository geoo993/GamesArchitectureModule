using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// https://www.pinterest.co.uk/pin/614671049115390417/

namespace WareHouse3
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


        string JingleBellsNote = "EEE EEE EGCD E F F FFF EE EG G FD C";
        
        string TwinkleLittleStarNote = "CCGGAAG FFEEDDC GGFFEED GGFFEED CCGGAAG FFEEDDC";
        
        string RainRainGoAwayNote = "GEGGEGGE AGGE FFDDFFD GFEDECC";
       
        string IncyIncySpiderNote = "CCC DEE E DCD EC E E FG GF EFGE CC DEE D CD EC C CCC DE E E DCD EC";
        
        public string GetSong(SongType type) {
            switch (type) {
            case SongType.JingleBells:
                    return JingleBellsNote;
            case SongType.TwinkleLittle:
                    return TwinkleLittleStarNote;
            case SongType.RainRainGoAway:
                    return RainRainGoAwayNote;
            case SongType.IncyIncySpider:
                    return IncyIncySpiderNote;
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
