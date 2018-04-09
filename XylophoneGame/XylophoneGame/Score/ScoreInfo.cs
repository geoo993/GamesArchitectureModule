using System;
using System.Collections.Generic;


namespace XylophoneGame
{
    public struct ScoreInfo
    {
        public string Song;
        public int Progress;
        public float TimeProgress;
        
        public float SongProgressSpeed;
        public int CollectedItems;
        
        public bool DidMatch;
        public int Errors;
        public int TotalErrors;
        public int Matches;
        
        public bool HasSongEnded;
        public int MaxNotes;
       
        internal ScoreInfo(string song, float progressSpeed)
        {
			this.Song = song;
            this.SongProgressSpeed = progressSpeed;
            this.MaxNotes = 0;
            this.Progress = 0;
            this.TimeProgress = 0.0f;
            this.Matches = 0;
            this.Errors = 0;
            this.TotalErrors = 0;
            this.DidMatch = false;
            this.CollectedItems = 0;
            this.HasSongEnded = false;
        }
        
    }
   
}
