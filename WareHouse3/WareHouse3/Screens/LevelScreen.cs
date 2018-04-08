using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace WareHouse3
{
    public class LevelScreen: Screen
    {
        /// <summary>
        /// Game title
        /// </summary>
        public string GameTitle;
        
        /// <summary>
        /// HUD
        /// </summary>
        public HUD Hud;
        
        
        /// <summary>
        /// Level.
        /// </summary>
        public Level Level;
        public String LevelTitle;

        /// <summary>
        /// Song.
        /// </summary>
        public SongType SongType;
        public string Song {
            get {
                return XylophoneSongs.Instance.GetSong(SongType);
            }
        }
        public float SongProgressSpeed = 20.0f;
        public bool AutoPlay = false;
        
        public LevelScreen(string title, string level, SongType song, ScreensType type, ScreenManager parent, ContentManager contentManager)
        : base(type, parent, contentManager)
        {
            SongType = song;
            GameTitle = title;
            LevelTitle = level;
            Hud = new HUD(title);
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

            LoadLevel();
			Hud.Construct(Song, ContentManager, Color.DarkMagenta, GameInfo.Instance.RandomColor());
			Hud.Subscribe(Level.ScoreSubject);
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void OnExit()
        {
            Destroy();
            base.OnExit();
        }
        
        private void LoadLevel()
        {
            // Unloads the content for the current level before loading the next one.
            if (Level != null)
                Level.Dispose();

            // Load the level.
            string levelPath = string.Format("Content/Levels/"+LevelTitle+".txt");
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                Level = new Level(Parent.Services, fileStream, SongType, Song);
                
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetCurrentLevel(string level)
        {
            LevelTitle = level;
            LoadLevel();
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            Level.Update(gameTime, new Vector2(GameInfo.MapWidth, GameInfo.MapHeight), SongProgressSpeed, AutoPlay, Hud.Width);
            
            Hud.Update(gameTime, Level.Progress, Level.TimeProgress / 100.0f);
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Draw(SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
            BackgroundTexture = CreateTexture(screenSafeArea.Width, screenSafeArea.Height, Color.Ivory);
            base.Draw(spriteBatch, screenSafeArea);

            Hud.Draw(spriteBatch, screenSafeArea);//, Level.ScoreSubject);// Level.DidMatch, Level.Matches, Level.Errors);
            
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
