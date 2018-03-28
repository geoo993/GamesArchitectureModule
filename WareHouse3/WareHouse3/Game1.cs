using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.Tracing;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using NLua;
using TexturePackerLoader;

namespace WareHouse3
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Fields
        private readonly TimeSpan timePerFrame = TimeSpan.FromSeconds(1f/30f); 
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
		Vector2 centreScreen;
        Vector2 screenSize;
        Ball ball;
        //List<Tile> tiles;
        private Level level;
        
         #endregion
        
        public Game1()
        {
        
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            

#if WINDOWS_PHONE
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            graphics.IsFullScreen = true;
#else
            // set the backbuffer size to something that will work well on both xbox
            // and windows.
            graphics.PreferredBackBufferWidth = GameInfo.ScreenWidth;
            graphics.PreferredBackBufferHeight = GameInfo.ScreenHeight;
#endif

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Device.graphicsDevice = this.GraphicsDevice;
            GameInfo.Camera.ViewportWidth = graphics.GraphicsDevice.Viewport.Width;
            GameInfo.Camera.ViewportHeight = graphics.GraphicsDevice.Viewport.Height;

            InitializeControlsBindings();
            base.Initialize();
        }

        private void InitializeControlsBindings()
        {
            Commands.manager.AddKeyboardBinding(Keys.Escape, StopGame);
            
        }
        
        #region Game Actions
        public void StopGame(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                Exit();
            }
        }

        #endregion
        
        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            centreScreen = new Vector2 (this.GraphicsDevice.Viewport.Width / 2, this.GraphicsDevice.Viewport.Height / 2);
            screenSize = new Vector2 (this.GraphicsDevice.Viewport.Width, this.GraphicsDevice.Viewport.Height);

            ball = new Ball(new Vector2(400, GameInfo.MapHeight), 20, 8.0f, 5.0f, GameInfo.Instance.RandomColor());
		    ball.SetKeyoardBindings();
            //SetupObstacles();
            
            GameInfo.Camera.CenterOn(ball);
            GameInfo.Camera.SetKeyoardBindings();

            LoadLevel();
        }
        
        private void LoadLevel()
        {
            // Unloads the content for the current level before loading the next one.
            if (level != null)
                level.Dispose();

            // Load the level.
            string levelPath = string.Format("Content/Levels/level.txt");
            using (Stream fileStream = TitleContainer.OpenStream(levelPath))
                level = new Level(Services, fileStream);
        }
        
		void SetupObstacles () {
			
			//tiles = new List<Tile>();
			//for (int i = 0; i < ObstatclesInfo.NumberOfObstacles; i++) {
			//	tiles.Add( new Tile(
            //    new Vector2(GameInfo.Random.Next(0, GameInfo.MapWidth), GameInfo.Random.Next(0, GameInfo.MapHeight)), GameInfo.Random.Next(20, 100), GameInfo.Random.Next(20, 100), (float)GameInfo.Random.Next(-(int)ObstatclesInfo.MaxSpeed, (int)ObstatclesInfo.MaxSpeed)/20.0f, 0.0f, RandomColor(GameInfo.Random)) );
			//}
		}
		
         /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        
        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update the command manager (updates polling input and fires input events)
            Commands.manager.Update();
            var mapSize = new Vector2(GameInfo.MapWidth, GameInfo.MapHeight);
            
            //ball.UpdateCollisions(tiles, mapSize);
			ball.UpdatePosition(gameTime, mapSize);

            //UpdateObstaclesMovement(tiles);
            
    //        foreach (Tile tile in tiles)
    //        {
				//tile.UpdatePosition(gameTime, mapSize);
            //}
            
            GameInfo.Camera.CenterOn(ball, false);
            //GameInfo.Camera.UpdateInputs();
            
            //System.Diagnostics.Debug.Print("Camera Position "+ GameInfo.Camera.Position.ToString());
            // TODO: Add your update logic here
            
            // update our level, passing down the GameTime along with all of our input states
            level.Update(gameTime);
            
            
            base.Update(gameTime);
        }
        
        
        public void UpdateObstaclesMovement(List<Tile> tiles) 
        {   
            if (tiles.Count <= 0 ) {
                return;
            }
            
            Tile tempBall1;            
            Tile tempBall2;
            
            
            for (int i = 0; i < tiles.Count; i++)
            {
                
                tempBall1 = tiles[i];

                for (int k = 0; k < tiles.Count; k++)
                {
                    tempBall2 = tiles[k];
                    
                    if (tempBall1 == tempBall2) continue;

                    if (CircularCollision(tempBall1, tempBall2))
                    {
                        CollideBalls(tempBall1, tempBall2);
                        
                        if(CircularCollision(tempBall1,tempBall2))
                        {
                            
                            tempBall1.MoveSpeed *= -1;
                            tempBall2.MoveSpeed *= -1;
                            
                            // go in different directions
                            tempBall1.Position += tempBall1.MoveSpeed;
                            
                            tempBall2.Position -= tempBall2.MoveSpeed;
                        }
                        
                    }
                }

            }
            
        }
        
        public void CollideBalls(Circle circle1, Circle circle2)
        {
            float dx = circle1.Position.X - circle2.Position.X;
            float dy = circle1.Position.Y - circle2.Position.Y;
            float collisionAngle = (float)Math.Atan2(dy, dx);
            
            var speed1 = Math.Sqrt( (circle1.MoveSpeed.X * circle1.MoveSpeed.X) + (circle1.MoveSpeed.Y * circle1.MoveSpeed.Y));
            var speed2 = Math.Sqrt( (circle2.MoveSpeed.X * circle2.MoveSpeed.X) + (circle2.MoveSpeed.Y * circle2.MoveSpeed.Y));
            
            var direction1 = Math.Atan2(circle1.MoveSpeed.Y, circle1.MoveSpeed.X);
            var direction2 = Math.Atan2(circle2.MoveSpeed.Y, circle2.MoveSpeed.X);
            
            var velocityX1 = speed1 * Math.Cos(direction1 - collisionAngle);
            var velocityY1 = speed1 * Math.Sin(direction1 - collisionAngle);         
            var velocityX2 = speed2 * Math.Cos(direction2 - collisionAngle);
            var velocityY2 = speed2 * Math.Sin(direction2 - collisionAngle);
            
            var mass1 = (circle1.Width / 2);
            var mass2 = (circle2.Width / 2);
            
            var finalVelocityX1 = ((mass1 - mass2) * velocityX1 + (mass2 + mass2) * velocityX2) / (mass1 + mass2);
            var finalVelocityX2 = ((mass1 + mass1) * velocityX1 + (mass2 - mass1) * velocityX2) / (mass1 + mass2);
            
            var finalVelocityY1 = velocityY1;
            var finalVelocityY2 = velocityY2;
            
            circle1.MoveSpeed.X = (float)(Math.Cos(collisionAngle) * finalVelocityX1 + Math.Cos(collisionAngle + Math.PI / 2) * finalVelocityY1);
            circle1.MoveSpeed.Y = (float)(Math.Sin(collisionAngle) * finalVelocityX1 + Math.Sin(collisionAngle + Math.PI / 2) * finalVelocityY1);
            circle2.MoveSpeed.X = (float)(Math.Cos(collisionAngle) * finalVelocityX2 + Math.Cos(collisionAngle + Math.PI / 2) * finalVelocityY2);
            circle2.MoveSpeed.Y = (float)(Math.Sin(collisionAngle) * finalVelocityX2 + Math.Sin(collisionAngle + Math.PI / 2) * finalVelocityY2);
            
            circle1.Position += circle1.MoveSpeed;
            circle2.Position += circle2.MoveSpeed;
        }
     
        private bool CircularCollision(Circle mc1, Circle mc2)
        {
            var radiusOfBoth = mc1.Radius + mc2.Radius;
            var distance = getDistance(mc1.Position, mc2.Position);
            
            return (distance <= radiusOfBoth);
        }
        
        private float getDistance(Vector2 start, Vector2 end)
        {
           
            float dx = end.X - start.X;
            float dy = end.Y - start.Y;
            
            float distance = (float)Math.Sqrt( dx * dx + dy * dy);
            return distance;
        }
        
        
        
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

            //TODO: Add your drawing code here
            //this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            this.spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, GameInfo.Camera.TranslationMatrix );

            ball.Render(spriteBatch);
            
            level.Draw(gameTime, spriteBatch);
    //        foreach (Tile tile in tiles)
    //        {
				//tile.Render(spriteBatch);
            //}
            
            this.spriteBatch.End();

            base.Draw(gameTime);
        
        }
       
    }
}
