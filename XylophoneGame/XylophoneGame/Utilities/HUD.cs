using System;
using System.Diagnostics;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XylophoneGame
{
    public class HUD: ScoreObserver
    {

        /// <summary>
        /// Song
        /// </summary>
		private string Song;
        private int LengthOfSong;
        private char CurrentNote;
        private string NotesCompleted;
        
        private string NotesCompletedScore;
        private List<char> ScoreList;
        
        /// <summary>
        /// HUD elements
        /// </summary>
        public Progressbar HudProgressBar;

        /// <summary>
        /// Text color
        /// </summary>
        private Color HudTextColor;
        private Color HudTextProgressColor;
        
        /// <summary>
        /// The flash score.
        /// </summary>
        private bool FlashScore;
        private float FlashScoreCount;
        
		private FrameCounter FrameCounter;
        
        public HUD(string title)
        : base(title+" Score System")
        {
            HudProgressBar = new Progressbar();
            FrameCounter = new FrameCounter();
        }
        
        public void Construct(Color textColor, Color textProgressColor)
        {
            HudTextColor = textColor;
            HudTextProgressColor = textProgressColor;
            NotesCompletedScore = "";
			ScoreList = new List<char>();
            FlashScore = false;
        }
   
        public void Update(GameTime gameTime)
        {
            
            FrameCounter.Update(gameTime);
        }
        
        // Update information.
        public override void OnNext(ScoreInfo value) 
        {
        
            Song = value.Song;
            LengthOfSong = Song.Length;
            CurrentNote = Song[value.Progress];
            NotesCompleted = Song.Substring(0, value.Progress);
            FlashScore = value.HasSongEnded;
            
            //Debug.Print("");
            //Debug.Print("Auto play " + value.AutoPlay);
            //Debug.Print("Song length " + LengthOfSong);
            //Debug.Print("Progress " + value.Progress);
            //Debug.Print("Time Progress " + value.TimeProgress);
            //Debug.Print("has Ended " + value.HasSongEnded);
        
			HudProgressBar.SetProgress(value.Progress, LengthOfSong);
			HudProgressBar.SetTimeProgress(value.TimeProgress);
            
            // score text
            if (value.DidMatch)
            {
                if (NotesCompleted.Length == 0) {
                    ScoreList.Clear();
                }
                
                NotesCompletedScore = "";
                int length = NotesCompleted.Length - ScoreList.Count;
                for (int i = -1; i < length; i++)
                {
                    if (i == length - 1)
                    {
                        var score = (value.Errors == 0) ? 'X' : '0';
                        ScoreList.Add(score);
                    } else {
                        ScoreList.Add(' ');
                    }
                }
                foreach(char c in ScoreList){
                    NotesCompletedScore += c;
                }   
            }
            
            base.OnNext(value);
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Draw(SpriteBatch spriteBatch, float width, float height, Vector2 hudPosition, SpriteFont hudFont, SpriteFont hudLargeFont)
        {
            
            var NotesCompletedWidth = hudFont.MeasureString(NotesCompleted).X;
            var notesPosition = hudPosition + new Vector2(-(width * 0.5f), 0.0f);
            
            // xylophone notes
            ScreenManager.DrawShadowedString(spriteBatch, hudFont, Song, notesPosition, Vector2.Zero, HudTextColor, 1.0f);
            ScreenManager.DrawShadowedString(spriteBatch, hudFont, CurrentNote.ToString(), notesPosition + new Vector2(NotesCompletedWidth, height * 1.2f), Vector2.Zero, HudTextProgressColor, 1.0f);

            //ScreenManager.DrawShadowedString(spriteBatch, HudFont, score, GameInfo.Camera.Position, HudTextColor, 1.0f);
            //ScreenManager.DrawShadowedString(spriteBatch, HudFont, NotesCompletedScore, notesPosition + new Vector2(0.0f, height * 1.2f), HudTextColor, 1.0f);
            FlashScoreCount = FlashScore ? (FlashScoreCount + 0.05f) % 1.0f : 0.2f;
            
            var Max = MaxNotes - 1;
            var word = Matches.ToString() +"/"+Max.ToString();
            var largetFontWidth = hudLargeFont.MeasureString(word).X;
            var color = HudTextColor * FlashScoreCount;
            var origin = new Vector2(largetFontWidth / 2, 0);
            ScreenManager.DrawShadowedString(spriteBatch, hudLargeFont, word, GameInfo.Camera.Position - new Vector2(0.0f, 100.0f), origin, color, 1.0f);
            
			// hud frames
			HudProgressBar.Construct((int)width, (int)height, hudPosition - new Vector2((width * 0.5f), 0.0f), Color.Orange, Color.Gold, Color.Thistle);
			HudProgressBar.Draw(spriteBatch);
			
            // frame rate
            //var fps = string.Format("FPS: {0}", FrameCounter.AverageFramesPerSecond);
            //DrawShadowedString(spriteBatch, HudFont, fps, new Vector2(hudLocation.X - (safeArea.Width / 2.0f), hudLocation.Y), hudColor, hudColor);
        }

        public void Destroy() {
         
            HudProgressBar.Destroy();
            HudProgressBar = null;

            ScoreList.Clear();
            base.OnCompleted();
        }
    }
}
