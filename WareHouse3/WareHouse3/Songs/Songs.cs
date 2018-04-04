using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        List<string> JingleBells = new List<string> {
            "NoteE", "NoteE", "NoteE", 
            "NoteE", "NoteE", "NoteE", 
            "NoteE", "NoteG", "NoteC", "NoteD", 
            "NoteE", 
            "NoteF", 
            "NoteF", "NoteF", "NoteF", "NoteF", "NoteE", "NoteE", 
            "NoteG", "NoteG", 
            "NoteG", 
            "NoteF", "NoteD", 
            "NoteC"
        };

        string TwinkleLittleStarNote = "CCGGAAG FFEEDDC GGFFEED GGFFEED CCGGAAG FFEEDDC";
        List<string> TwinkleLittleStar = new List<string> {
            "NoteC", "NoteC", "NoteG", "NoteG", "NoteA", "NoteA", "NoteG", 
            "NoteF", "NoteF", "NoteE", "NoteE", "NoteD", "NoteD", "NoteC",
            "NoteG", "NoteG", "NoteF", "NoteF", "NoteE", "NoteE", "NoteD", 
            "NoteG", "NoteG", "NoteF", "NoteF", "NoteE", "NoteE", "NoteD", 
            "NoteC", "NoteC", "NoteG", "NoteG", "NoteA", "NoteA", "NoteG", 
            "NoteF", "NoteF", "NoteE", "NoteE", "NoteD", "NoteD", "NoteC"
        };
        
        
        string RainRainGoAwayNote = "GEGGEGGE AGGE FFDDFFD GFEDECC";
        List<string> RainRainGoAway = new List<string> {
            "NoteG", "NoteE", "NoteG", "NoteG", "NoteE", "NoteG", "NoteG", "NoteE", 
            "NoteA", "NoteG", "NoteG", "NoteE",
            "NoteF", "NoteF", "NoteD", "NoteD", "NoteF", "NoteF", "NoteD",
            "NoteG", "NoteF", "NoteE", "NoteD", "NoteE", "NoteC", "NoteC", 
        };
        
        
        string IncyIncySpiderNote = "CCC DEE E DCD EC E E FG GF EFGE CC DEE D CD EC C CCC DE E E DCD EC";
        List<string> IncyIncySpider = new List<string> {
            "NoteC", "NoteC", "NoteC", "NoteD", "NoteE", "NoteE", 
            "NoteE", "NoteD", "NoteC", "NoteD", "NoteE", "NoteC",
            "NoteE", "NoteE", "NoteF", "NoteG",
            "NoteG", "NoteF", "NoteE", "NoteF", "NoteG", "NoteE", 
            "NoteC", "NoteC", "NoteD", "NoteE", "NoteE",
            "NoteD", "NoteC", "NoteD", "NoteE", "NoteC", "NoteC",
            "NoteC", "NoteC", "NoteC", "NoteD", "NoteE", "NoteE", 
            "NoteE", "NoteD", "NoteC", "NoteD", "NoteE", "NoteC",
        };
        
        public string GetNextElement(IList<string> list, int index)
        {
            var newIndex = (index) % list.Count;
            return list[newIndex];
        }
        
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
        
        public List<string> GetSongNotes(SongType type) {
            switch (type) {
            case SongType.JingleBells:
                    return JingleBells;
            case SongType.TwinkleLittle:
                    return TwinkleLittleStar;
            case SongType.RainRainGoAway:
                    return RainRainGoAway;
            case SongType.IncyIncySpider:
                    return IncyIncySpider;
            default:
                    return null;
            }
        }
    }
    
}
