using System.Collections;
using System.Collections.Concurrent;

namespace Comercial_Office.Model
{
    public class Office
    {

        public string Identificator { get; set; }
        public ConcurrentQueue<string> UserQueue { get; set; }

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

        //TODO arreglar return
        public long IsAvailable()
        {
            if (this.AttentionPlaceList != null)
            {
                foreach (AttentionPlace attentionPlace in this.AttentionPlaceList)
                {
                    if (attentionPlace.IsAvailable) 
                    {
                        return attentionPlace.Number;
                    }
                }
                return 0;
            }
            return 0;
        }

        //Ver como modificar el tema de que sea vacio.
        public void OcupyAttentionPlace(long postId)
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
