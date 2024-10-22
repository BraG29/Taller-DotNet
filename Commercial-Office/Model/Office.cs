using System.Collections.Concurrent;

namespace Commercial_Office.Model
{
    public class Office
    {

        public string Identificator { get; set; }

        public class TimedQueueItem<T>
        {
            public T Item { get; set; }
            public DateTime EnqueuedTime { get; set; }

            public TimedQueueItem(T item)
            {
                Item = item;
                EnqueuedTime = DateTime.UtcNow; // Almacena la hora en que el objeto fue encolado
            }
        }

        public ConcurrentQueue<TimedQueueItem<string>> UserQueue { get; set; } 

        public IList<AttentionPlace> AttentionPlaceList { get; set; }

        public Office (string identificator, ConcurrentQueue<TimedQueueItem<string>> userQueue, IList<AttentionPlace> attentionPlaceList)
        {
            this.Identificator = identificator;
            this.UserQueue = userQueue;
            this.AttentionPlaceList = attentionPlaceList;
        }

        public long IsAvailable()
        {
            if (this.AttentionPlaceList != null)
            {
                foreach (AttentionPlace attentionPlace in this.AttentionPlaceList)
                {
                    if (attentionPlace.IsAvailable) 
                    {
                        return (long)attentionPlace.Number;
                    }
                }
                return -1;
            }
            return -1;
        }

        public void OcupyAttentionPlace(ulong postId)
        {
            if (this.AttentionPlaceList != null)
            {
                foreach (AttentionPlace attentionPlace in this.AttentionPlaceList)
                {
                    if (attentionPlace.Number == postId) 
                    {
                        attentionPlace.IsAvailable = false;
                    }
                }
            }
        }

    }
}
