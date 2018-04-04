using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

// https://stackoverflow.com/questions/20676185/xna-monogame-getting-the-frames-per-second

namespace WareHouse3
{
    public class FrameCounter
    {
        
        public static bool MustSkipFrame(float waitMilliseconds, TimeSpan currentTime, TimeSpan previousTime)
        {
            return ((waitMilliseconds > 0.0f) && (currentTime.TotalMilliseconds - previousTime.TotalMilliseconds < waitMilliseconds));
        }
        
        
        public FrameCounter()
        {
        }
       
       
        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; } // frame Rate
    
        public const int MAXIMUM_SAMPLES = 100;
    
        private Queue<float> _sampleBuffer = new Queue<float>();
    
        public virtual bool Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            CurrentFramesPerSecond = 1.0f / deltaTime;
            
            _sampleBuffer.Enqueue(CurrentFramesPerSecond);
    
            if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                _sampleBuffer.Dequeue();
                AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            } 
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }
    
            TotalFrames++;
            TotalSeconds += deltaTime;
            return true;
        }
    }
    
}
