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

       /* public void OcupyAttentionPlace(ulong postId)
        {
            if (this.AttentionPlaceList != null)
            {
                foreach (AttentionPlace attentionPlace in this.AttentionPlaceList)
                {
                    if (attentionPlace.Number == postId) 
                    {
                        if (!attentionPlace.IsAvailable)
                        {
                            throw new ArgumentException($"El puesto no esta libre");
                        }

                        attentionPlace.IsAvailable = false;
                    }
                }

                //throw new KeyNotFoundException($"No existe dicho puesto" + postId);
            }
        }*/

    }
}
