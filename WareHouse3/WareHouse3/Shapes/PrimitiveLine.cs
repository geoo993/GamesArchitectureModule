using System;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// http://xboxforums.create.msdn.com/forums/t/7414.aspx
// https://blogs.msdn.microsoft.com/manders/2007/01/07/lines-2d-thick-rounded-line-segments-for-xna-programs/

namespace WareHouse3
{
    /// <summary>
    /// A class to make primitive 2D objects out of lines.
    /// </summary>
    public class PrimitiveLine
    {
        Texture2D pixel;
        ArrayList vectors;

        /// <summary>
        /// Gets/sets the colour of the primitive line object.
        /// </summary>
        public Color Colour;

        /// <summary>
        /// Gets/sets the position of the primitive line object.
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Gets/sets the render depth of the primitive line object (0 = front, 1 = back)
        /// </summary>
        public float Depth;

        /// <summary>
        /// Gets the number of vectors which make up the primtive line object.
        /// </summary>
        public int CountVectors
        {
            get
            {
                return vectors.Count;
            }
        }

        /// <summary>
        /// Creates a new primitive line object.
        /// </summary>
        /// <param name="graphicsDevice">The Graphics Device object to use.</param>
        public PrimitiveLine(GraphicsDevice graphicsDevice, Color color)
        {
            // create pixels
            //int radius = 50;
            this.Colour = color;
            this.Position = new Vector2(0, 0);
            this.Depth = 0;

            vectors = new ArrayList();
           
            pixel = new Texture2D(graphicsDevice, 1, 1, true, SurfaceFormat.Color);
            //pixel = new Texture2D(graphicsDevice, 1, 1, true, TextureUsage.None, SurfaceFormat.Color);
            Color[] pixels = new Color[1];
            pixels[0] = Color.White;
            pixel.SetData<Color>(pixels);

            
        }

        /// <summary>
        /// Called when the primive line object is destroyed.
        /// </summary>
        ~PrimitiveLine()
        {
        }

        /// <summary>
        /// Adds a vector to the primive live object.
        /// </summary>
        /// <param name="vector">The vector to add.</param>
        public void AddVector(Vector2 vector)
        {
            vectors.Add(vector);
        }

        /// <summary>
        /// Insers a vector into the primitive line object.
        /// </summary>
        /// <param name="index">The index to insert it at.</param>
        /// <param name="vector">The vector to insert.</param>
        public void InsertVector(int index, Vector2 vector)
        {
            vectors.Insert(index, vectors);
        }

        /// <summary>
        /// Removes a vector from the primitive line object.
        /// </summary>
        /// <param name="vector">The vector to remove.</param>
        public void RemoveVector(Vector2 vector)
        {
            vectors.Remove(vector);
        }

        /// <summary>
        /// Removes a vector from the primitive line object.
        /// </summary>
        /// <param name="index">The index of the vector to remove.</param>
        public void RemoveVector(int index)
        {
            vectors.RemoveAt(index);
        }

        /// <summary>
        /// Clears all vectors from the primitive line object.
        /// </summary>
        public void ClearVectors()
        {
            vectors.Clear();
        }
        
        /// <summary>
        /// Creates a circle starting from 0, 0.
        /// </summary>
        /// <param name="radius">The radius (half the width) of the circle.</param>
        /// <param name="sides">The number of sides on the circle (the more the detailed).</param>
        public void CreateCircle(float radius, int sides)
        {
            vectors.Clear();

            float max = 2 * (float)Math.PI;
            float step = max / (float)sides;

            for (float theta = 0; theta < max; theta += step)
            {
                vectors.Add(new Vector2(radius * (float)Math.Cos((double)theta),
                    radius * (float)Math.Sin((double)theta)));
            }

            // then add the first vector again so it's a complete loop
            vectors.Add(new Vector2(radius * (float)Math.Cos(0),
                    radius * (float)Math.Sin(0)));
        }
        
        
        /// <summary> 
        /// Create a line box  
        /// </summary> 
        /// <param name="topLeft">Top Left hand corner of the box</param> 
        /// <param name="botRight">Bottom Right hand coner of the box</param> 
        public void CreateBox(Vector2 topLeft, Vector2 botRight) 
        { 
            vectors.Clear(); 
 
            vectors.Add(topLeft); 
            vectors.Add(new Vector2(topLeft.X, botRight.Y)); 
 
            vectors.Add(botRight); 
            vectors.Add(new Vector2(botRight.X, topLeft.Y)); 
 
            vectors.Add(topLeft); 
        
        } 
        
        
        /// <summary>
        /// Renders the primtive line object.
        /// </summary>
        /// <param name="spriteBatch">The sprite batch to use to render the primitive line object.</param>
        public void Render(SpriteBatch spriteBatch, float thickness, Color? color = null)
        {
            if (color != null) {
                Colour = color.Value;
            }
            
            if (vectors.Count < 2)
                return;

            for (int i = 1; i < vectors.Count; i++)
            {
                Vector2 vector1 = (Vector2)vectors[i-1];
                Vector2 vector2 = (Vector2)vectors[i];

                // calculate the distance between the two vectors
                float distance = Vector2.Distance(vector1, vector2);

                // calculate the angle between the two vectors
                float angle = (float)Math.Atan2((double)(vector2.Y - vector1.Y),
                    (double)(vector2.X - vector1.X));

                spriteBatch.Draw(
                    pixel,
                    Position + vector1 + 0.5f * (vector2 - vector1),
                    null, 
                    Colour,  
                    angle,  
                    new Vector2(0.5f, 0.5f),  
                    new Vector2(distance, thickness),  
                    SpriteEffects.None,  
                    Depth); 
            }
        }
        
    }
}
