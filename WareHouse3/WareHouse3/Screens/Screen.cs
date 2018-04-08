﻿using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WareHouse3
{
    public class Screen
    {
        public ScreensType Type { get; private set; }
        protected ScreenManager Parent { get; private set; }

        protected ContentManager ContentManager { get; private set; }

        public Color BackgroundColor { get; private set; }
        public Texture2D BackgroundTexture;
        
        protected bool IsBlocked { get; private set; }
        
        
        public Screen(ScreensType type, ScreenManager parent, ContentManager contentManager) 
        {
            Type = type;
            Parent = parent;

            ContentManager = contentManager;

            BackgroundColor = Color.Black;
            BackgroundTexture = null;

        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public virtual void Construct(Color backgroundColor, Texture2D backgroundTexture)
        {
            
            BackgroundColor = backgroundColor;
            BackgroundTexture = backgroundTexture;

            IsBlocked = false;
        }
        
        public virtual void SetKeyoardBindings(CommandManager manager) 
        {
            
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public virtual void Destroy()
        {
           
            BackgroundTexture = null;

            ContentManager = null;

            Parent = null;
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetScreenType(ScreensType type)
        {
            Type = type;
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void SetBackgroundColor(Color color)
        {
            BackgroundColor = color;
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Block()
        {
            IsBlocked = true;
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public void Unblock()
        {
           IsBlocked = false;
        }
        
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public virtual void Update(GameTime gameTime)
        {
            if (IsBlocked) {
            
            } else {
                
            }
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public virtual void Draw(SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
            
            if (BackgroundTexture != null)
            {
                Vector2 hudLocation = new Vector2(GameInfo.Camera.Position.X - (screenSafeArea.Width / 2.0f), GameInfo.Camera.Position.Y - (screenSafeArea.Height / 2.0f) );
                spriteBatch.Draw(BackgroundTexture, hudLocation, Color.White);
            }
        }
        
        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public virtual void OnEnter()
        {
            
        }

        //-----------------------------------------------------------------------------
        //
        //-----------------------------------------------------------------------------
        public virtual void OnExit()
        {
        }
        
    }
}
