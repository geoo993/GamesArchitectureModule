#region File Description
//-----------------------------------------------------------------------------
// Level.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Input;
using TexturePackerLoader;

namespace WareHouse3
{
    /// <summary>
    /// A uniform grid of tiles with collections of gems and enemies.
    /// The level owns the player and controls the game's win and lose
    /// conditions as well as scoring.
    /// </summary>
    class Level 
    {
        private Tile[,] tiles; 
        private Ball Ball;  
        
        // Level game state.
        private Random random = new Random(354668); // Arbitrary, but constant seed
        
         // Level content.        
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;
        
        private Loader loader;
        
        
        #region Bounds and collision

        /// <summary>
        /// Gets the collision mode of the tile at a particular location.
        /// This method handles tiles outside of the levels boundries by making it
        /// impossible to escape past the left or right edges, but allowing things
        /// to jump beyond the top of the level and fall off the bottom.
        /// </summary>
        public TileCollision GetCollision(int x, int y)
        {
            
            // Prevent escaping past the level ends.
            if (x < 0 || x >= Width)
                return TileCollision.Impassable;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= Height)
                return TileCollision.Passable;

            return tiles[x, y].Collision;
        }

        /// <summary>
        /// Gets the bounding rectangle of a tile in world space.
        /// </summary>        
        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * TileInfo.UnitWidth, y * TileInfo.UnitHeight, TileInfo.UnitWidth, TileInfo.UnitHeight);
        }

        /// <summary>
        /// Width of level measured in tiles.
        /// </summary>
        public int Width
        {
            get { return tiles.GetLength(0); }
        }

        /// <summary>
        /// Height of the level measured in tiles.
        /// </summary>
        public int Height
        {
            get { return tiles.GetLength(1); }
        }

        #endregion

        
        #region Loading

        /// <summary>
        /// Constructs a new level.
        /// </summary>
        /// <param name="serviceProvider">
        /// The service provider that will be used to construct a ContentManager.
        /// </param>
        /// <param name="fileStream">
        /// A stream containing the tile data.
        /// </param>
        public Level(IServiceProvider serviceProvider, Stream fileStream, CommandManager manager)
        {
            // Create a new content manager to load content used just by this level.
            content = new ContentManager(serviceProvider, "Content");
            
            loader = new Loader(fileStream);
            
            SetKeyoardBindings(manager);
            LoadTiles(fileStream);
        }
        
         private void SetKeyoardBindings(CommandManager manager) 
        {
            manager.AddKeyboardBinding(Keys.Left, LeftMovement);
            manager.AddKeyboardBinding(Keys.Right, RightMovement);
            manager.AddKeyboardBinding(Keys.Up, UpMovement);
            manager.AddKeyboardBinding(Keys.Down, DownMovement);
            manager.AddKeyboardBinding(Keys.Space, JumpMovement);
        }
    
        void LeftMovement(ButtonAction buttonState, Vector2 amount)
        {
            Ball.IsLeft(buttonState);
            
            if (buttonState == ButtonAction.DOWN)
            {
                
            }
            
            if (buttonState == ButtonAction.UP)
            {
               
            } 
        }
        
        void RightMovement(ButtonAction buttonState, Vector2 amount)
        {
            Ball.IsRight(buttonState);
            
            if (buttonState == ButtonAction.DOWN)
            {
                
            } 
            
            if (buttonState == ButtonAction.UP)
            {
                
            } 
        }
        
        void UpMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                Ball.IsScaledUp();
            }
        }
        
        
        void DownMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                Ball.IsScaledDown();
            }
        }
   
        void JumpMovement(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.PRESSED)
            {
                Ball.IsJump();
            }
        }
     
        /// <summary>
        /// Iterates over every tile in the structure file and loads its
        /// appearance and behavior. This method also validates that the
        /// file is well-formed with a player start point, exit, etc.
        /// </summary>
        /// <param name="fileStream">
        /// A stream containing the tile data.
        /// </param>
        private void LoadTiles(Stream fileStream)
        {
            // Load the level and ensure all of the lines are the same length.
            int width;
            List<string> lines = new List<string>();
            
            /*
            using (StreamReader reader = new StreamReader(fileStream))
            {
                string line = reader.ReadLine();
                width = line.Length;
                while (line != null)
                {
                    lines.Add(line);
                    if (line.Length != width)
                        throw new Exception(String.Format("The length of line {0} is different from all preceeding lines.", lines.Count));
                    line = reader.ReadLine();
                }
            }
            */
            
             //StreamReader code removed here
             
            lines = loader.ReadLinesFromTextFile();
            width = lines[0].Length;
            
            // Allocate the tile grid.
            tiles = new Tile[width, lines.Count];

            // Loop over every tile position,
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // to load each tile.
                    char tileType = lines[y][x];
                    var tile = LoadTile(tileType, x, y);
                    if ( tile != null) {
						tiles[x, y] = tile;
                    }
                }
            }
            
            System.Diagnostics.Debug.Print("Width "+ Width.ToString()); // 20
            System.Diagnostics.Debug.Print("Height "+ Height.ToString()); // 15

        }
        
        
        /// <summary>
        /// Loads an individual tile's appearance and behavior.
        /// </summary>
        /// <param name="tileType">
        /// The character loaded from the structure file which
        /// indicates what should be loaded.
        /// </param>
        /// <param name="x">
        /// The X location of this tile in tile space.
        /// </param>
        /// <param name="y">
        /// The Y location of this tile in tile space.
        /// </param>
        /// <returns>The loaded tile.</returns>
        private Tile LoadTile(char tileType, int x, int y)
        {
            switch (tileType)
            {
                 // Ball
                case '1':
                    return LoadPlayerTile(x, y);
                    
                // Blank space
                case '.':
                    return LoadEmptyTile();
                // Platform block
                case '~':
                    return LoadVarietyTile("BlockB", x, y, TileCollision.Platform);

                // Passable block
                case ':':
                    return LoadVarietyTile("BlockB", x, y, TileCollision.Passable);

                // Impassable block
                case '#':
                    return LoadVarietyTile("BlockA", x, y, TileCollision.Impassable);

                // Unknown tile type character
                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }
        
        private Tile LoadEmptyTile()
        {
            return null;//new Tile(Vector2.Zero, 1, 1, 0.0f, 0.0f, Color.Transparent, null, TileCollision.Passable);
        }
        
        
        /// <summary>
        /// Creates a new tile. The other tile loading methods typically chain to this
        /// method after performing their special logic.
        /// </summary>
        /// <param name="name">
        /// Path to a tile texture relative to the Content/Tiles directory.
        /// </param>
        /// <param name="collision">
        /// The tile collision type for the new tile.
        /// </param>
        /// <returns>The new tile.</returns>
        private Tile LoadTile(string name, Vector2 position, TileCollision collision)
        {
            //var texture = Content.Load<Texture2D>("Tiles/" + name);
            return new Box(position, TileInfo.UnitWidth, TileInfo.UnitHeight, 0.0f, 0.0f, GameInfo.Instance.RandomColor(), null, collision);
            
        }
        
        /// <summary>
        /// Loads a tile with a random appearance.
        /// </summary>
        /// <param name="baseName">
        /// The content name prefix for this group of tile variations. Tile groups are
        /// name LikeThis0.png and LikeThis1.png and LikeThis2.png.
        /// </param>
        /// <param name="variationCount">
        /// The number of variations in this group.
        /// </param>
        private Tile LoadVarietyTile(string name, int x, int y, TileCollision collision)
        {
            Point position = GetBounds(x, y).Center;
            return LoadTile(name, new Vector2(position.X, position.Y), collision);
        }
        
        /// <summary>
        /// Instantiates a player, puts him in the level, and remembers where to put him when he is resurrected.
        /// </summary>
        private Tile LoadPlayerTile(int x, int y)
        {
            if (Ball != null)
                throw new NotSupportedException("A level may only have one starting point.");

            Rectangle rect = GetBounds(x, y);
            Vector2 start = RectangleExtensions.GetBottomCenter(rect);
            
            Ball = new Ball(start, rect.Width, 8.0f, 5.0f, GameInfo.Instance.RandomColor(), null, TileCollision.Passable);
            GameInfo.Camera.CenterOn(Ball);


            return Ball;
        }
        
        /// <summary>
        /// Unloads the level content.
        /// </summary>
        public void Dispose()
        {
            Content.Unload();
        }

        #endregion
        
        #region Update

        /// <summary>
        /// Updates all objects in the world, performs collision between them,
        /// and handles the time limit with scoring.
        /// </summary>
        public void Update(GameTime gameTime, Vector2 mapSize)
        {
            GameInfo.Camera.CenterOn(Ball, false);
            Ball.UpdatePosition(gameTime, mapSize);
            
        }
        #endregion
        
        #region Draw

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
           
            DrawTiles(spriteBatch);
			Ball.Render(spriteBatch);

        }

        /// <summary>
        /// Draws each tile in the level.
        /// </summary>
        private void DrawTiles(SpriteBatch spriteBatch)
        {
            // For each tile position
            for (int y = 0; y < Height; ++y)
            {
                for (int x = 0; x < Width; ++x)
                {
                    // If there is a visible tile in that position
                    var tile = tiles[x, y];
                    if (tile != null) {
						tile.Render(spriteBatch);
                    }
                    
                }
            }
        }

        #endregion
        
    }
}