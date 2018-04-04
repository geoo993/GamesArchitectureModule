using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WareHouse3
{
    public class LevelScreen: Screen
    {
        public Level CurrentLevel { get; private set; }
        
        public LevelScreen(ScreensType type, ScreenManager parent, ContentManager contentManager)
        : base(type, parent, contentManager)
        {
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Construct(Color backgroundColor, Texture2D backgroundTexture)
        {
            base.Construct(backgroundColor, backgroundTexture);

            //mFSM = new StateMachine(this, 0.0f, null);
            //mUpdateWaitMilliseconds = 0.0f;
         
            // Set first state inside OnEnter() method
            //SetState(GameStates.LEVEL_STATE_TUTORIAL);
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public virtual void SetCurrentLevel(string level)
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
        public override void Update(TimeSpan currentGameTime)
        {
            
            base.Update(currentGameTime);
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Draw(TimeSpan currentGameTime, SpriteBatch spriteBatch, Vector2 cameraLocation)
        {
            base.Draw(currentGameTime, spriteBatch, cameraLocation);

            if (CurrentLevel != null)
            {
                //CurrentLevel.Draw(currentGameTime, spriteBatch);
            }
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public override void Destroy()
        {
          
            CurrentLevel = null;

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
            
        }
        
    }
}
