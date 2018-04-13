using System;
using System.Collections.Generic;

using System.Linq;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XylophoneGame
{
    public class LevelScreen: Screen
    {
       
        /// <summary>
        /// HUD
        /// </summary>
        public HUD Hud;

        /// <summary>
        /// Level.
        /// </summary>
        public Level Level;
        
        public LevelScreen(ScreensType type, ScreenManager parent, ContentManager contentManager)
        : base(type, parent, contentManager)
        {
            Hud = new HUD(parent.Player);
			SaveLoadJSON.Load();

        }

        public void Construct(string level, SongType songType, Color backgroundColor, Texture2D backgroundTexture)
        {
            Construct(backgroundColor, backgroundTexture);
            
            var song = XylophoneSongs.Instance.GetSong(songType);
			LoadLevel(level, songType, song);
			
			Hud.Construct(Color.DarkMagenta, GameInfo.Instance.RandomColor());
			Hud.Subscribe(Parent.ScoreSubject);
            
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Construct(Color backgroundColor, Texture2D backgroundTexture)
        {
            base.Construct(backgroundColor, backgroundTexture);
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Destroy()
        {
            Level.Destroy();
            Level = null;

            Hud.Destroy();
            Hud = null;
            
            base.Destroy();
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void OnEnter()
        {
            base.OnEnter();

        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void OnExit()
        {
            Destroy();
            base.OnExit();
        }
        
        private void LoadLevel(string title, SongType songType, string song)
        {
            // Unloads the content for the current level before loading the next one.
            if (Level != null)
                Level.Dispose();

            // Load the level.
            string levelPath = string.Format("Content/Levels/"+title+".txt");
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                Level = new Level(Parent.Services, fileStream, songType, song);
                
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            Level.Update(gameTime, new Vector2(GameInfo.MapWidth, GameInfo.MapHeight), Parent.ScoreSubject, Parent.HudWidth);
            
            Hud.Update(gameTime);

        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Draw(SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
            BackgroundTexture = CreateTexture(screenSafeArea.Width, screenSafeArea.Height, Color.Ivory);
            base.Draw(spriteBatch, screenSafeArea);

            Hud.Draw(spriteBatch,  Parent.HudWidth, Parent.HudHeight, Parent.ScreenTopCenter, Parent.HudFont, Parent.HudLargeFont);
            
            if (Level != null)
            {
                Level.Draw(spriteBatch, screenSafeArea);
            }
        }
        
        
        #region helper functions
         
        public Texture2D CreateTexture(int Width, int Height, Color color) {
        
            Texture2D rectangle = new Texture2D(Device.graphicsDevice, Width, Height, false, SurfaceFormat.Color);
            
            Color[] colorData = new Color[Width * Height];
            for (int i = 0; i < Width * Height; i++)
                colorData[i] = color;
            rectangle.SetData<Color>(colorData);
            return rectangle;
        }
        
        #endregion
        
    }
}
