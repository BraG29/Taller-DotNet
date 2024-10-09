using System.Collections;

namespace Comercial_Office.Model
{
    public class Office
    {

        public string? Identificator { get; set; }
        public Queue? ClientQueue { get; set; }

        public IList<AttentionPlace>? AttentionPlaceList { get; set; }

        public Office()
        {

        }

        public Office (string identificator, Queue clientQueue, IList<AttentionPlace> attentionPlaceList)
        {
            this.Identificator = identificator;
            this.ClientQueue = clientQueue;
            this.AttentionPlaceList = attentionPlaceList;
        }

    }
}
