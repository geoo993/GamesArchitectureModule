using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
// https://roguesharp.wordpress.com/2014/07/13/tutorial-5-creating-a-2d-camera-with-pan-and-zoom-in-monogame/
// https://gamedev.stackexchange.com/questions/40272/how-can-i-implement-smooth-rotation-from-one-direction-to-another

namespace WareHouse3
{
    
    public class Camera
    {
       // Construct a new Camera class with standard zoom (no scaling)
       public Camera()
       {
            Zoom = 1.0f;
            SpriteWidth = 0;
            SpriteHeight = 0;
            CameraMovement = Vector2.Zero;
            IsMoving = false;
       }

        // Centered Position of the Camera in pixels.
        public Vector2 Position;
       // Current Zoom level with 1.0f being standard
       public float Zoom { get; private set; }
       // Current Rotation amount with 0.0f being standard orientation
       public float Rotation { get; private set; }
     
       // Height and width of the viewport window which we need to adjust
       // any time the player resizes the game window.
       public int ViewportWidth { get; set; }
       public int ViewportHeight { get; set; }

       private int SpriteWidth;
       private int SpriteHeight;
       private bool IsMoving;
        
       private Vector2 CameraMovement;
     
       // Center of the Viewport which does not account for scale
       public Vector2 ViewportCenter
       {
          get
          {
             return new Vector2( ViewportWidth * 0.5f, ViewportHeight * 0.5f );
          }
       }
     
       // Create a matrix for the camera to offset everything we draw,
       // the map and our objects. since the camera coordinates are where
       // the camera is, we offset everything by the negative of that to simulate
       // a camera moving. We also cast to integers to avoid filtering artifacts.
       public Matrix TranslationMatrix
       {
          get
          {
             return Matrix.CreateTranslation( -(int) Position.X,
                -(int) Position.Y, 0 ) *
                Matrix.CreateRotationZ( Rotation ) *
                Matrix.CreateScale( new Vector3( Zoom, Zoom, 1 ) ) *
                Matrix.CreateTranslation( new Vector3( ViewportCenter, 0 ) );
          }
       }
     
       // Call this method with negative values to zoom out
       // or positive values to zoom in. It looks at the current zoom
       // and adjusts it by the specified amount. If we were at a 1.0f
       // zoom level and specified -0.5f amount it would leave us with
       // 1.0f - 0.5f = 0.5f so everything would be drawn at half size.
       public void AdjustZoom( float amount )
       {
          Zoom += amount;
          if ( Zoom < 0.25f )
          {
             Zoom = 0.25f;
          }
       }
     
       // Move the camera in an X and Y amount based on the cameraMovement param.
       // if clampToMap is true the camera will try not to pan outside of the
       // bounds of the map.
       public void MoveCamera( Vector2 cameraMovement, bool clampToMap = false )
       {
          Vector2 newPosition = Position + cameraMovement;
     
          if ( clampToMap )
          {
             Position = MapClampedPosition( newPosition);
          }
          else
          {
             Position = newPosition;
          }
       }
     
       public Rectangle ViewportWorldBoundry()
       {
          Vector2 viewPortCorner = ScreenToWorld( new Vector2( 0, 0 ) );
          Vector2 viewPortBottomCorner =
             ScreenToWorld( new Vector2( ViewportWidth, ViewportHeight ) );
     
          return new Rectangle( (int) viewPortCorner.X,
             (int) viewPortCorner.Y,
             (int) ( viewPortBottomCorner.X - viewPortCorner.X ),
             (int) ( viewPortBottomCorner.Y - viewPortCorner.Y ) );
       }
     
       // Center the camera on specific pixel coordinates
       public void CenterOn( Vector2 position )
       {
          Position = position;
       }
     
       // Center the camera on a specific cell in the map
       public void CenterOn( Shape shape, bool smooth = false)
       {
            var newPosition = CenteredPosition(shape, true);
            if (smooth) {
				SmoothMovement(Position, newPosition, 5.0f);
            } else {
                Position = newPosition;
            }
          
       }
     
       private Vector2 CenteredPosition( Shape shape, bool clampToMap = false )
       {
            SpriteWidth = shape.Width;
            SpriteHeight = shape.Height;
            var cameraPosition = new Vector2( shape.Position.X, shape.Position.Y);
            var cameraCenteredOnTilePosition = new Vector2( cameraPosition.X + SpriteWidth / 2, cameraPosition.Y + SpriteHeight / 2 );
            if ( clampToMap )
            {
                return MapClampedPosition( cameraCenteredOnTilePosition);
            }
        
          return cameraCenteredOnTilePosition;
       }
     
       // Clamp the camera so it never leaves the visible area of the map.
       private Vector2 MapClampedPosition( Vector2 position)
       {
          var cameraMax = new Vector2( GameInfo.MapWidth - ( ViewportWidth / Zoom / 2 ),
                                       GameInfo.MapHeight - ( ViewportHeight / Zoom / 2 ) );
     
          return Vector2.Clamp( position, new Vector2( ViewportWidth / Zoom / 2, ViewportHeight / Zoom / 2 ), cameraMax );
       }
     
       public Vector2 WorldToScreen( Vector2 worldPosition )
       {
          return Vector2.Transform( worldPosition, TranslationMatrix );
       }
     
       public Vector2 ScreenToWorld( Vector2 screenPosition )
       {
          return Vector2.Transform( screenPosition,
              Matrix.Invert( TranslationMatrix ) );
       }
     
        public void ScrollLeft(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                CameraMovement.X = -1;
            }
        }
        public void ScrollRight(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                CameraMovement.X = 1;
            }
        }
        public void ScrollUp(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                CameraMovement.Y = -1;
            }
        }
        public void ScrollDown(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                CameraMovement.Y = 1;
            }
        }
        public void ZoomOut(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                AdjustZoom( 0.25f );
            }
        }
         
        public void ZoomIn(ButtonAction buttonState, Vector2 amount)
        {
            if (buttonState == ButtonAction.DOWN)
            {
                AdjustZoom( -0.25f );
            }
        }


       // Move the camera's position based on input
       public void SetKeyoardBindings() 
        {
            Commands.manager.AddKeyboardBinding(Keys.A, ScrollLeft);
            Commands.manager.AddKeyboardBinding(Keys.D, ScrollRight);
            Commands.manager.AddKeyboardBinding(Keys.W, ScrollUp);
            Commands.manager.AddKeyboardBinding(Keys.S, ScrollDown);
            Commands.manager.AddKeyboardBinding(Keys.OemPeriod, ZoomOut);
            Commands.manager.AddKeyboardBinding(Keys.OemComma, ZoomIn);
        }
        
       public void UpdateInputs()
       {
          
          // When using a controller, to match the thumbstick behavior,
          // we need to normalize non-zero vectors in case the user
          // is pressing a diagonal direction.
          if ( CameraMovement != Vector2.Zero )
          {
             CameraMovement.Normalize();
          }
     
          // scale our movement to move 25 pixels per second
          CameraMovement *= 25f;
     
          MoveCamera( CameraMovement, true );
       }
       
       public void SmoothMovement(Vector2 start, Vector2 end, float speed ) 
       {
            // https://gamedev.stackexchange.com/questions/23447/moving-from-ax-y-to-bx1-y1-with-constant-speed
            float elapsed = 0.01f;
            
            // On starting movement
            float distance = (float)Math.Sqrt( Math.Pow(end.X - start.X, 2)+ Math.Pow(end.Y - start.Y, 2) );
            float directionX = (end.X - start.X) / distance;
            float directionY = (end.Y - start.Y) / distance;
            Position.X = start.X;
            Position.Y = start.Y;
            IsMoving = true;
            
            // On update
            if(IsMoving == true)
            {
                Position.X += directionX * speed * elapsed;
                Position.Y += directionY * speed * elapsed;
                if(Math.Sqrt(Math.Pow(Position.X - start.X, 2) + Math.Pow(Position.Y-start.Y, 2)) >= distance)
                {
                    Position.X = end.X;
                    Position.Y = end.Y;
                    IsMoving = false;
                }
            }
       }
       
       
    }
}
