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

        //private ScoreObserver ScoreObserver;
        
        /// <summary>
        /// Song
        /// </summary>
		private string Song;
        
        private int LengthOfSong
        {
            get { return Song.Length; }
        }

        private int CurrentNoteIndex;
        
        private char CurrentNote {
            get{
                return Song[CurrentNoteIndex];
            }
        } 
        
        private string NotesCompleted {
            get {
                return Song.Substring(0, CurrentNoteIndex);
            }
        }

        private string NotesCompletedScore;
        private List<char> ScoreList;
        
        /// <summary>
        /// HUD elements
        /// </summary>
        private SpriteFont HudFont;
        public Progressbar HudProgressBar;
        public Vector2 HudPosition;

        /// <summary>
        /// Text color
        /// </summary>
        private Color HudTextColor;
        private Color HudTextProgressColor;
        
        public float Width {
            get {
                if (HudFont != null)
                {
                    return HudFont.MeasureString(Song).X;
                }else {
                    return 0.0f;
                }
            }
        }
        
        public float NotesCompletedWidth {
            get {
                if (HudFont != null)
                {
                    return HudFont.MeasureString(NotesCompleted).X;
                }else {
                    return 0.0f;
                }
            }
        }
        
        private float Height {
            get {
                if (HudFont != null)
                {
                    return HudFont.MeasureString(Song).Y;
                }else {
                    return 0.0f;
                }
            }
        }
        
		private FrameCounter FrameCounter;
        
        public HUD(string title)
        : base(title+" Score System")
        {
            HudFont = null;
            HudProgressBar = new Progressbar();
            FrameCounter = new FrameCounter();
        }
        
        public void Construct(string song, ContentManager contentManager, Color textColor, Color textProgressColor)
        {
            Song = song;
            HudFont = contentManager.Load<SpriteFont>("Fonts/GameFont");
            HudPosition = Vector2.Zero;
            HudTextColor = textColor;
            HudTextProgressColor = textProgressColor;
            NotesCompletedScore = "";
			ScoreList = new List<char>();
        }
   
        public void Update(GameTime gameTime, int progress, float timeProgress)
        {
            CurrentNoteIndex = progress;
            HudProgressBar.Update(gameTime);
            
			HudProgressBar.SetProgress(MathExtensions.Percentage(progress + 1, LengthOfSong, 0));
            HudProgressBar.SetTimeProgress(timeProgress);
            
            FrameCounter.Update(gameTime);

        }
        
        // Update information.
        public override void OnNext(ScoreInfo value) 
        {
            
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
        public void Draw(SpriteBatch spriteBatch, Rectangle safeArea)
        {
        
            HudPosition = new Vector2(GameInfo.Camera.Position.X, GameInfo.Camera.Position.Y - (safeArea.Height / 2.0f));
            var notesPosition = HudPosition + new Vector2(-(Width * 0.5f), 0.0f);
            
            // xylophone notes
            DrawShadowedString(spriteBatch, HudFont, Song, notesPosition, HudTextColor, 1.0f);
            DrawShadowedString(spriteBatch, HudFont, CurrentNote.ToString(), notesPosition + new Vector2(NotesCompletedWidth, Height * 1.2f), HudTextProgressColor, 1.0f);

            Color scoreColor = HudTextColor;
            //DrawShadowedString(spriteBatch, HudFont, score, GameInfo.Camera.Position, scoreColor, 1.0f);
			DrawShadowedString(spriteBatch, HudFont, NotesCompletedScore, notesPosition + new Vector2(0.0f, Height * 1.2f), scoreColor, 1.0f);
            
            // hud frames
            HudProgressBar.Construct((int)Width, (int)Height, HudPosition - new Vector2((Width * 0.5f), 0.0f), Color.Orange, Color.Gold, Color.Thistle);
            HudProgressBar.Draw(spriteBatch);
            
            // frame rate
            //var fps = string.Format("FPS: {0}", FrameCounter.AverageFramesPerSecond);
            //DrawShadowedString(spriteBatch, HudFont, fps, new Vector2(hudLocation.X - (safeArea.Width / 2.0f), hudLocation.Y), hudColor, hudColor);
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color, float scale)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, value, position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }
        
        
        public void Destroy() {
            HudFont = null;

            HudProgressBar.Destroy();
            HudProgressBar = null;

            ScoreList.Clear();
            base.OnCompleted();
        }
    }
}
