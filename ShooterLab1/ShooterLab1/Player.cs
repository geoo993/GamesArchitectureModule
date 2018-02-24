using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;

namespace ShooterLab1.MacOS
{
    public class Player
    {
        
        // Animation representing the player
        public Animation PlayerAnimation;

        // Position of the Player relative to the upper left side of the screen
        public Vector2 Position;

        // State of the player
        public bool Active;

        // Amount of hit points that player has
        public int Health;

        // Get the width of the player ship
        public int Width
        {
            get { return PlayerAnimation.FrameWidth; }
        }

        // Get the height of the player ship
        public int Height
        {
            get { return PlayerAnimation.FrameHeight; }
        }

        // Initialize the player
        public void Initialize(Animation animation, Vector2 position)
        {
            PlayerAnimation = animation;

            // Set the starting position of the player around the middle of the screen and to the back
            Position = position;

            // Set the player to be active
            Active = true;
            
            
            //var folderName = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            //// Our executable is inside /MonoBundle
            //// So we need to go back one folder and enter "Resources/Content/Scripts/player.lua
            //state.DoFile("../Resources/Content/Scripts/GameManager.lua");
            ////var res = state.DoString("return 200")[0];

            var healthLua = GameManager.manager["HealthLua"];
            
            System.Diagnostics.Debug.Print(healthLua.ToString());

            double healthAsDouble = (double)healthLua;
            System.Diagnostics.Debug.Print(healthAsDouble.ToString());

            int healthAsInteger = (int)healthAsDouble;
            System.Diagnostics.Debug.Print(healthAsInteger.ToString());

            // Set the player health
            Health = healthAsInteger;
        }

        // Update the player animation
        public void Update(GameTime gameTime)
        {
            PlayerAnimation.Position = Position;
            PlayerAnimation.Update(gameTime);
        } 
        
        // Draw the Player
        public void Draw(SpriteBatch spriteBatch)
        {
            PlayerAnimation.Draw(spriteBatch); 
        }
    }
}