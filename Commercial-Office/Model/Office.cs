using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;

namespace Commercial_Office.Model
{
    public class Office
    {
        [Key]
        [Required]
        public string Identificator { get; set; }

        //public ConcurrentQueue<TimedQueueItem<string>> UserQueue { get; set; } 

        public IList<AttentionPlace> AttentionPlaceList { get; set; }

        public Office() 
        { 
            
        }

        public Office (string identificator, IList<AttentionPlace> attentionPlaceList)
        {
            this.Identificator = identificator;
            this.AttentionPlaceList = attentionPlaceList;
        }

    }
}
