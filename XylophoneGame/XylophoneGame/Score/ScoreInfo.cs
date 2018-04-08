using System;
using System.Collections.Generic;


namespace XylophoneGame
{
    public struct ScoreInfo
    {

        public bool DidMatch;
        public int Errors;
        public int TotalErrors;
        public int Matches;
        public int MaxScore;
       
        internal ScoreInfo(int maxScore)
        {
			this.MaxScore = maxScore;
            this.Matches = 0;
            this.Errors = 0;
            this.TotalErrors = 0;
            this.DidMatch = false;
        }
    }
   
}
