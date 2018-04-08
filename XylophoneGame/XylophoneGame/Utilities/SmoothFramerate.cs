using System;
using Microsoft.Xna.Framework;

// https://stackoverflow.com/questions/20676185/xna-monogame-getting-the-frames-per-second

namespace XylophoneGame
{
    
    class SmoothFramerate
    {
        private int samples;
        private int currentFrame;
        private double[] frametimes;
        private double currentFrametimes;
    
        public double Framerate
        {
            get
            {
                return (samples / currentFrametimes);
            }
        }
    
        public SmoothFramerate(int Samples)
        {
            samples = Samples;
            currentFrame = 0;
            frametimes = new double[samples];
        }
        
        public void Update(GameTime gameTime)
        {
            double timeSinceLastFrame = gameTime.ElapsedGameTime.TotalSeconds;
            
            currentFrame++;
            if (currentFrame >= frametimes.Length) { currentFrame = 0; }
    
            currentFrametimes -= frametimes[currentFrame];
            frametimes[currentFrame] = timeSinceLastFrame;
            currentFrametimes += frametimes[currentFrame];
        }
    }
}
