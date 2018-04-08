#region File Description
//-----------------------------------------------------------------------------
// Level.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using System.Linq;
using System.IO;
using System.Diagnostics;

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;

namespace XylophoneGame
{
    /// <summary>
    /// A uniform grid of tiles with collections of gems and enemies.
    /// The level owns the player and controls the game's win and lose
    /// conditions as well as scoring.
    /// </summary>
    public class Level
    {
        public ScoreSubject ScoreSubject { get; private set; }
      
        private List<Tile> Tiles;
        public Ball Ball { get; private set; }

        public SongType CurrentSongType { get; private set; }
        private string CurrentSong;
      
        public int Progress { get; set; }
        public float TimeProgress { get; set; }
        
		private bool FirstUpdate;
		private double InitialTime;
        
        //public int Matches { get; private set; } 
        //public int Errors { get; private set; }
        //public bool DidMatch { get; private set; }
        
        
        // Level content.        
        private ContentManager Content;
        
        private Loader Loader;
        

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
            return ClosestNote(Tiles, new Vector2(x * TileInfo.UnitWidth, y * TileInfo.UnitHeight)).Collision;
        }

        /// <summary>
        /// Gets the bounding rectangle of a tile in world space.
        /// </summary>        
        public Rectangle GetBounds(int x, int y)
        {
            return new Rectangle(x * TileInfo.UnitWidth, y * TileInfo.UnitHeight, TileInfo.UnitWidth, TileInfo.UnitHeight);
        }
        
         
        public Tile ClosestNote(List<Tile> tiles, Vector2 position)
        {
            
            // https://stackoverflow.com/questions/33145365/what-is-the-most-effective-way-to-get-closest-target
            return tiles
                .Where(o => o is Note)
                .OrderBy(o => (o.Position - position).LengthSquared())
                .FirstOrDefault();
        }
        
        public Tile ClosestTimeItem(List<Tile> tiles, Vector2 position)
        {
            
            // https://stackoverflow.com/questions/33145365/what-is-the-most-effective-way-to-get-closest-target
            return tiles
                .Where(o => (o is TimeItem && ((TimeItem)o).IsEnabled))
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
        public Level(IServiceProvider serviceProvider, Stream fileStream, SongType songType, string song)
        {
            // Create a new content manager to load content used just by this level.
            ScoreSubject = new ScoreSubject();
            ScoreSubject.StartScoreSystem(50);
            
            Content = new ContentManager(serviceProvider, "Content");
            Loader = new Loader(fileStream);
            CurrentSong = song;
            CurrentSongType = songType;
            HorizontalLength = GameInfo.LevelHorizontalLength;
            VerticalLength = GameInfo.LevelVerticalLength;
            
            LoadTiles(fileStream);
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
         
            lines = Loader.ReadLinesFromTextFile();
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
                case '1':
					// Passable
                    return LoadPlayerTile(x, y, TileCollision.Passable);
                case '.':
					// Blank space
                    return LoadEmptyTile(TileCollision.Passable);
                case 'T':
					//// Passable
                    return LoadTimeItem("Collectable", x, y,  GameInfo.Instance.RandomColor(), TileCollision.Impassable);
                case '#':
					//// Platform block
                    return LoadNoteTile("Platform", x, y,  Color.White, TileCollision.Platform);
                default:
					// Unknown tile type character
                    throw new NotSupportedException(String.Format("Unsupported tile type character '{0}' at position {1}, {2}.", tileType, x, y));
            }
        }
        
        private Tile LoadEmptyTile(TileCollision collision)
        {
            return null;//new Box(Vector2.Zero, 1, 1, 0.0f, 0.0f, 1.0f, Color.Transparent, null, null, collision);
        }
        
        
        /// <summary>
        /// Creates a new note tile.
        /// </summary>
        private Tile LoadNoteTile(string name, int x, int y, Color color, TileCollision collision)
        {
            Texture2D texture; 
            SoundEffect note;
            Point position = GetBounds(x, y).Center;
            
            var randNote = NoteInfo.UniqueRandomValue(NoteInfo.AvailableNotes);
            var noteName = NoteInfo.KeyByValue(randNote);
                    
            //<- file creating stuff here -> 
            try { 
            //<-- try to load the file --> 
                texture = Content.Load<Texture2D>("Notes/"+ noteName);
                note = Content.Load<SoundEffect>("Sounds/"+ randNote);
            } catch {
                //<--print exception--> 
                texture = null;
                note = null;
            } 
            var Note = new Note(noteName+(Tiles.Count+1).ToString(), position.ToVector2(), TileInfo.UnitWidth, TileInfo.UnitHeight, 0.0f, 0.0f, 1.0f, color, note, texture, collision);
            Note.NoteName = noteName;
            return Note;
        }
        
        /// <summary>
        /// Create time collectable item.
        /// </summary>
        private Tile LoadTimeItem(string name, int x, int y, Color color, TileCollision collision)
        {
            Texture2D texture;
            Texture2D animationTexture;
            Point position = GetBounds(x, y).Center;
           
            //<- file creating stuff here -> 
            try { 
            //<-- try to load the file --> 
                texture = Content.Load<Texture2D>("Icons/timeIcon");
                animationTexture = Content.Load<Texture2D>("SpriteSheets/timeIconAnimation");
            } catch {
                //<--print exception--> 
                texture = null;
                animationTexture = null;
            }
            
            return new TimeItem(name, position.ToVector2(), CollectableInfo.Radius, 10, 0, 0, color, null, texture, animationTexture, collision);
        }
        
        
        /// <summary>
        /// Instantiates a player, puts him in the level, and remembers where to put him when he is resurrected.
        /// </summary>
        private Tile LoadPlayerTile(int x, int y, TileCollision collision)
        {
            if (Ball != null)
                throw new NotSupportedException("A level may only have one ball.");

            Rectangle rect = GetBounds(x, y);
            Vector2 start = RectangleExtensions.GetBottomCenter(rect);
            int radius = rect.Width / 5;
            Ball = new Ball("Ball", this, start, radius, BallInfo.BallSpeed, BallInfo.BallJumpSpeed, BallInfo.BallMass, GameInfo.Instance.RandomColor(), null, null, collision);
            
            return null;
        }
        
        /// <summary>
        /// Unloads the level content.
        /// </summary>
        public void Dispose()
        {
            Content.Unload();
        }
        
        public void Destroy()
        {
            foreach(Tile tile in Tiles) {
                tile.Destroy();
            }
            Tiles.Clear();
        
            Ball.Destroy();
            Ball = null;

            ScoreSubject.EndScoreSystems();
            ScoreSubject = null;
            
            Dispose();
        }


        #endregion

        #region Update

        /// <summary>
        /// Updates all objects in the world, performs collision between them,
        /// and handles the time limit with scoring.
        /// </summary>
        public void Update(GameTime gameTime, Vector2 mapSize, float progressSpeed, bool autoPlay, float hudWidth)
        {
            // center camera on ball
            GameInfo.Camera.CenterOn(Ball, false);
            
            //Update Ball And Note Tiles
            Ball.UpdatePosition(gameTime, mapSize);
			
			if (FirstUpdate == false) {
				InitialTime = gameTime.TotalGameTime.TotalSeconds;
				FirstUpdate = true;
			}
            var totalTime = gameTime.TotalGameTime.TotalSeconds - InitialTime;
			var elapsed = (float)(totalTime * progressSpeed) % hudWidth;
            var closestNote = (Note)ClosestNote(Tiles, Ball.Position);
            var closestTimeItem = (TimeItem)ClosestTimeItem(Tiles, Ball.Position);
            var noteToSelect = XylophoneSongs.Instance.GetNoteName(CurrentSong[Progress]);
            var isNoteSelectedWithMatch = false;
            var isNoteSelectedWithError = false;

            ScoreSubject.ResetErrorsOnDidMatch();
            ScoreSubject.Matched(false);
            
            TimeProgress = MathExtensions.Percentage(elapsed, hudWidth, 0);
            Ball.Update(gameTime, mapSize, ref noteToSelect, ref isNoteSelectedWithMatch, ref isNoteSelectedWithError, ref closestNote, ref closestTimeItem);
            
            
            if (isNoteSelectedWithMatch)
            {
                ScoreSubject.AddMatches();
                ScoreSubject.Matched(true);
            }

            if (isNoteSelectedWithError)
                ScoreSubject.AddError();
            
            if (autoPlay)
            {
                // if update the note regardless of the player, we play automatically
                var timeProgressValue = (int)MathExtensions.Value(TimeProgress, CurrentSong.Length, 0.0f);
				var characterAtIndex = CurrentSong[timeProgressValue];
                
                if (Progress != timeProgressValue && characterAtIndex != ' ') 
                {
					Progress = timeProgressValue;
                
                    var getNextCharacterIndex = XylophoneSongs.Instance.GetIndexOfNextNote(CurrentSongType, Progress);
                    var nextCharacterAtIndex = CurrentSong[getNextCharacterIndex];
                    noteToSelect = XylophoneSongs.Instance.GetNoteName(nextCharacterAtIndex);
                    
                    var noteAsset = NoteInfo.AvailableNotes[noteToSelect];
                    var noteSound = Content.Load<SoundEffect>("Sounds/" + noteAsset);
                    noteSound.Play();
                }
            }
            else
            {
                
                // if ball has collided with the note to selct, we then go to the next note
                if (isNoteSelectedWithMatch && Ball.NoteSelected != null)
                {
                    Progress = (Progress + 1) % CurrentSong.Length;
                    Progress = XylophoneSongs.Instance.GetIndexOfNextNote(CurrentSongType, Progress);
                    
					var nextCharacterAtIndex = CurrentSong[Progress];
                    noteToSelect = XylophoneSongs.Instance.GetNoteName(nextCharacterAtIndex);
                }
            }
            
            
            for (int i = 0; i < Tiles.Count; ++i)
            {
                
                
                if (Ball.NoteSelected != null && noteToSelect != null && Tiles[i] is Note)
                {
					var note = (Note)Tiles[i];
                    
                    // give a random note to each notetile
					var noteName = (GameInfo.Random.Next(10) > 6) ? NoteInfo.KeyByValue(NoteInfo.UniqueRandomValue(NoteInfo.AvailableNotes)) : noteToSelect;
                    var noteAsset = NoteInfo.AvailableNotes[noteName];

                    note.Name = noteName+(i+1).ToString();
                    note.NoteName = noteName;
                    note.HasTexture = true;
                    note.Color = Color.White;
                    note.Texture = Content.Load<Texture2D>("Notes/" + noteName);
                    note.NoteSound = Content.Load<SoundEffect>("Sounds/" + noteAsset);
                }
                
                Tiles[i].UpdatePosition(gameTime, mapSize);
            }

        }
        #endregion
        
        #region Draw

        /// <summary>
        /// Draw everything in the level from background to foreground.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, Rectangle screenSafeArea)
        {
            
            foreach(Tile tile in Tiles) {
                if (tile != null) {
                    tile.Draw(spriteBatch, screenSafeArea);
                }
            }
            
			Ball.Draw(spriteBatch, screenSafeArea);
        }

        #endregion
        
    }
}