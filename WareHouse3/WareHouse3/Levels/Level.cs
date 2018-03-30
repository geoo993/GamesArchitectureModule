﻿#region File Description
//-----------------------------------------------------------------------------
// Level.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Linq;

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
    public class Level 
    {
        public List<Tile> Tiles;
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
            if (x < 0 || x >= HorizontalLength)
                return TileCollision.Impassable;
            // Allow jumping past the level top and falling through the bottom.
            if (y < 0 || y >= VerticalLength)
                return TileCollision.Passable;

            //return tiles[x, y].Collision;
            return ClosestTile(Tiles, new Vector2(x * TileInfo.UnitWidth, y * TileInfo.UnitHeight)).Collision;
        }

        /// <summary>
        /// Gets the bounding rectangle of a tile in world space.
        /// </summary>        
        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * TileInfo.UnitWidth, y * TileInfo.UnitHeight, TileInfo.UnitWidth, TileInfo.UnitHeight);
        }
        
         
        public Tile ClosestTile(List<Tile> tiles, Vector2 position)
        {
            // https://stackoverflow.com/questions/33145365/what-is-the-most-effective-way-to-get-closest-target
            return tiles
                .OrderBy(o => (o.Position - position).LengthSquared())
                .FirstOrDefault();
        }

        /// <summary>
        /// horizontal length distance of the level measured in tiles.
        /// </summary>
        private int HorizontalLength;

        /// <summary>
        /// vertical length distance of the level measured in tiles.
        /// </summary>
        private int VerticalLength;

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
            HorizontalLength = GameInfo.LevelHorizontalLength;
            VerticalLength = GameInfo.LevelVerticalLength;
            
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
            //manager.AddKeyboardBinding(Keys.Escape, StopGame);
        }
        
        //public void StopGame(ButtonAction buttonState, Vector2 amount)
        //{
        //    if (buttonState == ButtonAction.DOWN)
        //    {
                
        //    }
        //}


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
            List<string> lines = new List<string>();
            Tiles = new List<Tile>();
         
            lines = loader.ReadLinesFromTextFile();
            // Loop over every tile position,
            for (int y = 0; y < VerticalLength; ++y)
            {
                for (int x = 0; x < HorizontalLength; ++x)
                {
                    char tileType = lines[y][x];
                    var tile = LoadTile(tileType, x, y);
                    if ( tile != null) {
                        Tiles.Add(tile);
                    }
                }
            }
            
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
                 // Passable
                case '1':
                    return LoadPlayerTile(x, y, TileCollision.Passable);
                // Blank space
                case '.':
                    return LoadEmptyTile(TileCollision.Passable);
                case 'A':
                    return LoadVarietyTile("NoteA", 6, x, y, Color.White, TileCollision.Platform);
                case 'B':
                    return LoadVarietyTile("NoteB", 7, x, y, Color.White, TileCollision.Platform);
                case 'C':
                    return LoadVarietyTile("NoteC", 1, x, y, Color.White, TileCollision.Platform);
                case 'K':
                    return LoadVarietyTile("NoteC2", 8, x, y, Color.White, TileCollision.Platform);
                case 'D':
                    return LoadVarietyTile("NoteD", 2, x, y, Color.White, TileCollision.Platform);
                case 'E':
                    return LoadVarietyTile("NoteE", 3, x, y, Color.White, TileCollision.Platform);
                case 'F':
                    return LoadVarietyTile("NoteF", 4, x, y, Color.White, TileCollision.Platform);
                case 'G':
                    return LoadVarietyTile("NoteG", 5, x, y, Color.White,  TileCollision.Platform);
                    // Passable block
                case '$':
                    return LoadVarietyTile("Collectable", 0, x, y,  GameInfo.Instance.RandomColor(), TileCollision.Passable);
                // Impassable block
                case '~':
                    return LoadVarietyTile("Trolley", 0,  x, y,  GameInfo.Instance.RandomColor(), TileCollision.Impassable);

                // Impassable block
                case ':':
                    return LoadVarietyTile("PackageBox", 0, x, y,  GameInfo.Instance.RandomColor(),  TileCollision.Impassable);

                // Platform block
                case '#':
                    return LoadVarietyTile("Platform", 0, x, y,  GameInfo.Instance.RandomColor(), TileCollision.Platform);

                // Unknown tile type character
                default:
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }
        
        private Tile LoadEmptyTile(TileCollision collision)
        {
            return null;//new Box(Vector2.Zero, 1, 1, 0.0f, 0.0f, 1.0f, Color.Transparent, null, null, collision);
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
        private Tile LoadTile(string name, int index, Vector2 position, Color color, TileCollision collision)
        {
            Texture2D texture; 
            SoundEffect note; 
            
            //<- file creating stuff here -> 
            try { 
            //<-- try to load the file --> 
                texture = Content.Load<Texture2D>("Notes/" + name);
                note = content.Load<SoundEffect>("Sounds/note"+index);
            } catch {
                //<--print exception--> 
                texture = null;
                note = null;
            } 
            
            return new Box(name+(Tiles.Count+1).ToString(), position, TileInfo.UnitWidth, TileInfo.UnitHeight, 0.0f, 0.0f, 1.0f, color, note, texture, collision);
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
        private Tile LoadVarietyTile(string name, int index, int x, int y, Color color, TileCollision collision)
        {
            Point position = GetBounds(x, y).Center;
            return LoadTile(name, index, new Vector2(position.X, position.Y), color, collision);
        }
        
        /// <summary>
        /// Instantiates a player, puts him in the level, and remembers where to put him when he is resurrected.
        /// </summary>
        private Tile LoadPlayerTile(int x, int y, TileCollision collision)
        {
            if (Ball != null)
                throw new NotSupportedException("A level may only have one starting point.");

            Rectangle rect = GetBounds(x, y);
            Vector2 start = RectangleExtensions.GetBottomCenter(rect);
            
            Ball = new Ball("Ball", this, start, rect.Width / 5, BallInfo.BallSpeed, BallInfo.BallJumpSpeed, 5.0f, GameInfo.Instance.RandomColor(), null, null, collision);

            return null;
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
            
            foreach (Tile tile in Tiles)
            {
            
                tile.UpdatePosition(gameTime, mapSize);
            }

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
          
            foreach(Tile tile in Tiles) {
                if (tile != null) {
                    tile.Render(spriteBatch);
                }
            }
        }

        #endregion
        
    }
}