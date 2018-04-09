using System;
using System.Collections.Generic;

// https://docs.microsoft.com/en-us/dotnet/standard/events/observer-design-pattern
// https://docs.microsoft.com/en-us/dotnet/standard/events/how-to-implement-an-observer
// https://msdn.microsoft.com/en-us/library/dd990377(v=vs.110).aspx

namespace XylophoneGame
{
    /*
    A provider or subject, which is the object that sends notifications to observers. 
    A provider is a class or structure that implements the IObservable<T> interface. 
    The provider must implement a single method, IObservable<T>.Subscribe, 
    which is called by observers that wish to receive notifications from the provider.
    */
    public class ScoreSubject: IObservable<ScoreInfo>
    {
        private List<IObserver<ScoreInfo>> observers;
        private ScoreInfo score;
        
        public ScoreSubject()
        {
            observers = new List<IObserver<ScoreInfo>>();
            //scores = new List<ScoreInfo>();
        }
        
        public IDisposable Subscribe(IObserver<ScoreInfo> observer)
        {
            // Check whether observer is already registered. If not, add it
           if (! observers.Contains(observer)) {
                observers.Add(observer);
                // Provide observer with existing data.
                //foreach (var item in scores)
                //   observer.OnNext(item);
                UpdateScore(score);
           }
           return new Unsubscriber<ScoreInfo>(observers, observer);
        }
        
        public void UpdateScore(Nullable<ScoreInfo> info)
        {
            foreach (var observer in observers) {
                if (! info.HasValue)
                    observer.OnError(new ScoreInfoUnknownException());
                else
                    observer.OnNext(info.Value);
            }
        }
        
		// new score system is assigned, so add new info object to list.
        public void StartScoreSystem(string song, float progressSpeed)
        {
            score = new ScoreInfo(song, progressSpeed);
            score.MaxNotes = XylophoneSongs.Instance.GetNumberOfNotesInSong(song);
            UpdateScore(score);
        }
        
        public void AddMatches()
        {
            score.Matches++;
            UpdateScore(score);
        }

        public void AddError()
        {
            score.Errors++;
            score.TotalErrors++;
            UpdateScore(score);
        }
        
        public void ClearErrors()
        {
            score.Errors = 0;
            //UpdateScore(score);
        }
        
        public void ResetErrorsOnDidMatch()
        {
            if (score.DidMatch){
                ClearErrors();
            }
        }
        
        public void Matched(bool didMatch)
        {
            score.DidMatch = didMatch;
            UpdateScore(score);
        }
        
        public void SetProgress(int progress)
        {
            
            if (score.HasSongEnded == false)
            {
                score.HasSongEnded = false;
                if ((progress >= score.Song.Length - 1)) {
					score.HasSongEnded = true;
                    score.IsGameSuccess = true;
                }
                score.Progress = progress;
            }
                
            if (score.HasSongEnded == true)
                score.SongProgressSpeed = 0;
            
			UpdateScore(score);
        }
        
        public void SetTimeProgress(float timeProgress)
        {
            if (score.HasSongEnded == false)
            {
                score.HasSongEnded = false;
                if (timeProgress >= 99.9)
                {
                    score.HasSongEnded = true;
                    score.IsGameSuccess = false;
                }
				score.TimeProgress = timeProgress;
                
            }

            if (score.HasSongEnded == true)
                score.SongProgressSpeed = 0;
            
			UpdateScore(score);
        }
        
        public void ReduceProgressSpeed()
        {
            //score.SongProgressSpeed -= 10.0f;
            //UpdateScore(score);
        }
       
        public void SwitchAutopPlay()
        {
            score.AutoPlay = !score.AutoPlay;
            UpdateScore(score);
        }
        
        public void EndScoreSystems()
        {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();
            
                observers.Clear();
        }
    
    }
    
    /*
    When the class is instantiated in the ScoreInfo.Subscribe method, 
    it is passed a reference to the observers collection and a reference to the observer that is added to the collection. 
    These references are assigned to local variables. 
    When the object's Dispose method is called, 
    it checks whether the observer still exists in the observers collection, and, if it does, removes the observer.
    */
    
    internal class Unsubscriber<ScoreInfo> : IDisposable
    {
        private List<IObserver<ScoreInfo>> _observers;
        private IObserver<ScoreInfo> _observer;
        
        internal Unsubscriber(List<IObserver<ScoreInfo>> observers, IObserver<ScoreInfo> observer)
        {
            this._observers = observers;
            this._observer = observer;
        }
        
        public void Dispose()
        {
            if (_observer != null && _observers.Contains(_observer))
                _observers.Remove(_observer);
        }
    }
    
    
    /*
    
    If the ScoreInfo value is null, the UpdateScore method instantiates a ScoreInfoUnknownException object, 
    which is shown in the following example. It then calls each observer's OnError method and passes it the ScoreInfoUnknownException object. 
    Note that ScoreInfoUnknownException derives from Exception, but does not add any new members.
    */
    public class ScoreInfoUnknownException : Exception
    {
       internal ScoreInfoUnknownException() 
       { }
    }

}
