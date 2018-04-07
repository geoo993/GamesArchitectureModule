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
        /// HUD
        /// </summary>
        public HUD Hud;
        
        
        /// <summary>
        /// Level.
        /// </summary>
        public Level Level;
        public SongType SongType = SongType.IncyIncySpider;
        public string Song {
            get {
                return XylophoneSongs.Instance.GetSong(SongType);
            }
        }
        public float SongProgressSpeed = 0.0f;
        public bool AutoPlay = false;
        
        public LevelScreen(ScreensType type, ScreenManager parent, ContentManager contentManager)
        : base(type, parent, contentManager)
        {
            Hud = new HUD();
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

			Hud.Construct(Song, ContentManager, Color.DarkMagenta, GameInfo.Instance.RandomColor());
            LoadLevel("level3");
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void OnExit()
        {
            Destroy();
            base.OnExit();
        }
        
        private void LoadLevel(string level)
        {
            // Unloads the content for the current level before loading the next one.
            if (Level != null)
                Level.Dispose();

            // Load the level.
            string levelPath = string.Format("Content/Levels/"+level+".txt");
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                Level = new Level(Parent.Services, fileStream, SongType, Song);
                
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetCurrentLevel(string level)
        {
            //switch (level)
            //{
            //    case GameStates.LEVEL_STATE_TUTORIAL:
            //        if (mTutorialLevel == null)
            //        {
            //            mTutorialLevel = new GameLevelTutorial(EntityID.GAME_LEVEL_TUTORIAL, this, mContentManager);
            //            mTutorialLevel.Construct();
            //        }
            //        mCurrentLevel = mTutorialLevel;
            //        break;
            //}

            //mCurrentLevel.OnEnter();
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
        public override void Draw(SpriteBatch spriteBatch, Vector2 screenCenter)
        {
            base.Draw(spriteBatch, screenCenter);
			
			Hud.Draw(spriteBatch, Parent.ScreenSafeArea);
            if (Level != null)
            {
                Level.Draw(spriteBatch);
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
