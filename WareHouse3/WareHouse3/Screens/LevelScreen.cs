using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WareHouse3
{
    public class LevelScreen: Screen
    {
        
        /// <summary>
        /// HUD
        /// </summary>
        private SpriteFont HudFont;
        public Progressbar HudProgressBar = null;
        private FrameCounter FrameCounter;
        
        
        /// <summary>
        /// Level.
        /// </summary>
        public Level Level;
        
        
        public LevelScreen(ScreensType type, ScreenManager parent, ContentManager contentManager)
        : base(type, parent, contentManager)
        {
            HudFont = null;
            HudProgressBar = new Progressbar();
            FrameCounter = new FrameCounter();
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
          
            Level = null;

            base.Destroy();
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void OnEnter()
        {
            base.OnEnter();
            
            
            HudFont = ContentManager.Load<SpriteFont>("Fonts/GameFont");

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
                Level = new Level(Parent.Services, fileStream);
                
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
            
            Level.Update(gameTime, new Vector2(GameInfo.MapWidth, GameInfo.MapHeight));
            HudProgressBar.Update(gameTime);
            FrameCounter.Update(gameTime);
    
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Draw(SpriteBatch spriteBatch, Vector2 screenCenter)
        {
            base.Draw(spriteBatch, screenCenter);
			
			DrawHud(Parent.ScreenSafeArea, spriteBatch);
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
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
         private void DrawHud(Rectangle safeArea, SpriteBatch spriteBatch)
        {
            
            Vector2 hudLocation = new Vector2(GameInfo.Camera.Position.X, GameInfo.Camera.Position.Y - (safeArea.Height / 2.0f) + 10);
            Color hudColor = Color.Yellow;

            // Draw time remaining. Uses modulo division to cause blinking when the
            // player is running out of time.
            string songName = XylophoneSongs.Instance.GetSong(Level.CurrentSong);
            Color hudBorderColor = Color.Yellow;
            
            float songNameWidth = HudFont.MeasureString(songName).X;
            float songNameHeight = HudFont.MeasureString(songName).Y;
            DrawShadowedString(spriteBatch, HudFont, songName, hudLocation + new Vector2(-(songNameWidth * 0.5f), 0.0f), hudColor, hudBorderColor);

            // Draw score
            DrawShadowedString(spriteBatch, HudFont, "SCORE: ", hudLocation + new Vector2(0.0f, songNameHeight * 1.2f), hudColor, hudBorderColor);
            
            
            HudProgressBar.Construct((int)songNameWidth, (int)songNameHeight, hudLocation - new Vector2(-(songNameWidth * 0.5f), 0.0f), Color.Orange, Color.SlateBlue);
            HudProgressBar.Draw(spriteBatch);
            
            //var fps = string.Format("FPS: {0}", FrameCounter.AverageFramesPerSecond);
    
            //DrawShadowedString(spriteBatch, HudFont, fps, new Vector2(hudLocation.X - (safeArea.Width / 2.0f), hudLocation.Y), hudColor, hudColor);
        }

        private void DrawShadowedString(SpriteBatch spriteBatch, SpriteFont font, string value, Vector2 position, Color color, Color borderColor)
        {
            spriteBatch.DrawString(font, value, position + new Vector2(1.0f, 1.0f), borderColor);
            spriteBatch.DrawString(font, value, position, color);
        }
        
        #endregion
        
    }
}
