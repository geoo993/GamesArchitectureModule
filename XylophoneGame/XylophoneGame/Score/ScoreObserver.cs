using System;

namespace XylophoneGame
{
    public class ScoreObserver : IObserver<ScoreInfo>
    {
        /*
        An observer, which is an object that receives notifications from a provider. 
        An observer is a class or structure that implements the IObserver<T> interface. 
        The observer must implement three methods, all of which are called by the provider:

            IObserver<T>.OnNext, which supplies the observer with new or current information.

            IObserver<T>.OnError, which informs the observer that an error has occurred.

            IObserver<T>.OnCompleted, which indicates that the provider has finished sending notifications.
        
        */
        private IDisposable unsubscriber;
        private ScoreInfo LatestScore;
        public string Name { get; private set; }
        
        public ScoreObserver(string name)
        {
            this.Name = name;
        }
        
        public virtual void Subscribe(IObservable<ScoreInfo> provider)
        {
          if (provider != null) 
             unsubscriber = provider.Subscribe(this);
        }
        
        public virtual void Unsubscribe()
        {
            unsubscriber.Dispose();
        }
        
        public virtual void OnCompleted() 
        {
            //Console.WriteLine("The score system has completed {0}.", this.Name);
            this.Unsubscribe();
        }
        
        // No implementation needed: Method is not called by the BaggageHandler class.
        public virtual void OnError(Exception e)
        {
            // No implementation.
            Console.WriteLine("The score {0} cannot be determined.", this.Name);
        }
        
        // Update information.
        public virtual void OnNext(ScoreInfo value) 
        {
            //Console.WriteLine("Errors {0}, Matches {1} of {2}.", value.Errors, value.Matches, this.Name);
            LatestScore = value;
        }
        
        public float ProgressSpeed {
            get { return LatestScore.SongProgressSpeed; }
        }
        
        public float DelayTimeProgress {
            get { return LatestScore.DelayTimeProgress; }
        }
        
        public int MaxNotes {
            get { return LatestScore.MaxNotes; }
        }
        
        public int Matches {
            get { return LatestScore.Matches; }
        }
        
        public bool HasSongEnded {
            get { return LatestScore.HasSongEnded; }
        }
        
        public bool IsGameSuccess {
            get { return LatestScore.IsGameSuccess; }
        }
   
        public bool AutoPlay {
            get { return LatestScore.AutoPlay; }
        }
   
    }
}
