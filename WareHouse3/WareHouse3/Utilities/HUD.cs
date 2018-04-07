using System;
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
        public SongType Type;
        private int LengthOfSong
        {
            get { return Song.Length; }
        }
        
        /// <summary>
        /// HUD elements
        /// </summary>
        private SpriteFont HudFont;
        public Progressbar HudProgressBar;
        private FrameCounter FrameCounter;

        /// <summary>
        /// Text color
        /// </summary>
        Color HudTextColor;
        Color HudTextBorderColor;
        
        public float ProgressAmount;
        
        public HUD()
        {
            HudFont = null;
            HudProgressBar = new Progressbar();
            FrameCounter = new FrameCounter();
        
        }
        
        public void Construct(SongType songType, Color textColor, ContentManager contentManager)
        {
            
			HudFont = contentManager.Load<SpriteFont>("Fonts/GameFont");
            HudTextColor = textColor;
            HudTextBorderColor = textColor;
            Type = songType;
            Song = XylophoneSongs.Instance.GetSong(songType);
        }
        
        float GetProgress() 
        {
            int index = (int)ProgressAmount % LengthOfSong;
            var getNextCharacterIndex = GetNextLetter(index, LengthOfSong);
            var character = Song[index];
            var nextCharacter = Song[getNextCharacterIndex];
            var progress = MathExtensions.Percentage(getNextCharacterIndex+1, LengthOfSong, 0);
            
            
            Debug.Print(" ");
            Debug.Print("index : "+index.ToString());
            Debug.Print("next index : "+getNextCharacterIndex.ToString());
            Debug.Print("character at: "+character.ToString());
            Debug.Print("next character at: "+nextCharacter.ToString());
            Debug.Print("Number of characters "+LengthOfSong.ToString());
            Debug.Print("percent at: "+progress.ToString());

            ProgressAmount = getNextCharacterIndex;
            return progress;
        }
        
        int GetNextLetter(int currentIndex, int max) {
            int index = 0;
            for (index = currentIndex; index < max; ++index) {
				var tempLetter = Song[index];
                if (tempLetter != ' ') {
                    break;
                }
            }
            return index;
        }
        
        public void Update(GameTime gameTime)
        {
            HudProgressBar.Update(gameTime);

            HudProgressBar.SetProgress(GetProgress());
            
            FrameCounter.Update(gameTime);
        }
        
		//-----------------------------------------------------------------------------
		//
		//-----------------------------------------------------------------------------
        public void Draw(SpriteBatch spriteBatch, Rectangle safeArea)
        {
        
            Vector2 hudLocation = new Vector2(GameInfo.Camera.Position.X, GameInfo.Camera.Position.Y - (safeArea.Height / 2.0f) + 10);
			float songNameWidth = HudFont.MeasureString(Song).X;
			float songNameHeight = HudFont.MeasureString(Song).Y;
            
            
            DrawShadowedString(spriteBatch, HudFont, Song, hudLocation + new Vector2(-(songNameWidth * 0.5f), 0.0f), HudTextColor, HudTextBorderColor);

            // Draw score
            //DrawShadowedString(spriteBatch, HudFont, "SCORE: ", hudLocation + new Vector2(0.0f, songNameHeight * 1.2f), HudTextColor, HudTextBorderColor);
            
			HudProgressBar.Construct((int)songNameWidth, (int)songNameHeight, hudLocation - new Vector2(-(songNameWidth * 0.5f), 0.0f), Color.Orange, Color.Gold, Color.SlateBlue);
			HudProgressBar.Draw(spriteBatch);
			
            
            //var fps = string.Format("FPS: {0}", FrameCounter.AverageFramesPerSecond);
    
            //DrawShadowedString(spriteBatch, HudFont, fps, new Vector2(hudLocation.X - (safeArea.Width / 2.0f), hudLocation.Y), hudColor, hudColor);
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color, Color borderColor)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), borderColor);
            spriteBatch.DrawString(font, value, position, color);
        }
    }
}
