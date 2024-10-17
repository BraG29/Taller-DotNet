using System.Collections;
using System.Collections.Concurrent;

namespace Commercial_Office.Model
{
    public class Office
    {

        public string Identificator { get; set; }
        public ConcurrentQueue<string> UserQueue { get; set; }
        //public ConcurrentQueue<TimedQueueItem<string>> UserQueue { get; set; } = new ConcurrentQueue<TimedQueueItem<string>>();

        public IList<AttentionPlace> AttentionPlaceList { get; set; }
        
        public Office()
        {

        }

        public Office (string identificator, ConcurrentQueue<string> userQueue, IList<AttentionPlace> attentionPlaceList)
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
