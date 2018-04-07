﻿using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.VisualBasic;

namespace WareHouse3
{
    public class HUD
    {
        
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
        
        public HUD()
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
        }
        
        /*
        float GetProgress() 
        {
            int index = (int)ProgressAmount % LengthOfSong;
            var getNextCharacterIndex = XylophoneSongs.Instance.GetIndexOfNextNote(Type, index);
            var character = Song[index];
            var nextCharacter = Song[getNextCharacterIndex];
            var charactersDone = Song.Substring(0, CurrentNoteIndex);
            var progress = MathExtensions.Percentage(getNextCharacterIndex+1, LengthOfSong, 0);
            
            
            //Debug.Print(" ");
            //Debug.Print("index : "+index.ToString());
            //Debug.Print("next index : "+CurrentNoteIndex.ToString());
            //Debug.Print("character at: "+character.ToString());
            //Debug.Print("next character at: "+CurrentNote.ToString());
            //Debug.Print("Characters done "+charactersDone);
            //Debug.Print("Number of characters "+LengthOfSong.ToString());
            //Debug.Print("percent at: "+progress.ToString());

            ProgressAmount = getNextCharacterIndex;
            return progress;
        }
        */
        
        
        public void Update(GameTime gameTime, int progress, float timeProgress)
        {
            CurrentNoteIndex = progress;
            HudProgressBar.Update(gameTime);
            
			HudProgressBar.SetProgress( MathExtensions.Percentage(progress + 1, LengthOfSong, 0));
            HudProgressBar.SetTimeProgress(timeProgress);
            
            FrameCounter.Update(gameTime);
        }
        
		//-----------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------
        public void Draw(SpriteBatch spriteBatch, Rectangle safeArea)
        {
        
            HudPosition = new Vector2(GameInfo.Camera.Position.X, GameInfo.Camera.Position.Y - (safeArea.Height / 2.0f) + 10);
            var notesPosition = HudPosition + new Vector2(-(Width * 0.5f), 0.0f);
            DrawShadowedString(spriteBatch, HudFont, Song, notesPosition, HudTextColor);
			//DrawShadowedString(spriteBatch, HudFont, NotesCompleted, notesPosition, HudTextProgressColor);
            
            DrawShadowedString(spriteBatch, HudFont, CurrentNote.ToString(), notesPosition + new Vector2(NotesCompletedWidth, Height * 1.2f), HudTextProgressColor);
            
            
            // Draw score
            //DrawShadowedString(spriteBatch, HudFont, "SCORE: ", hudLocation + new Vector2(0.0f, songNameHeight * 1.2f), HudTextColor);
            
            HudProgressBar.Construct((int)Width, (int)Height, HudPosition - new Vector2((Width * 0.5f), 0.0f), Color.Orange, Color.Gold, Color.SlateBlue);
            HudProgressBar.Draw(spriteBatch);
            
            //var fps = string.Format("FPS: {0}", FrameCounter.AverageFramesPerSecond);
            //DrawShadowedString(spriteBatch, HudFont, fps, new Vector2(hudLocation.X - (safeArea.Width / 2.0f), hudLocation.Y), hudColor, hudColor);
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), color);
            spriteBatch.DrawString(font, value, position, color);
        }
        
        
        
        public void Destroy() {
            HudFont = null;

            HudProgressBar.Destroy();
            HudProgressBar = null;
        }
    }
}
