using Commercial_Office.Model;
using System.Collections.Concurrent;
using static Commercial_Office.Model.Office;

namespace Commercial_Office.Infraestructure
{
    public class OfficeRepositoryImpl : IOfficeRepository
    {

        private IDictionary<string, Office> _Offices;

        public OfficeRepositoryImpl() { 
                
            _Offices = new Dictionary<string, Office>();

            //Objeto de prueba oficina1
            ConcurrentQueue<TimedQueueItem<string>> _OfficeQueue1 = new ConcurrentQueue<TimedQueueItem<string>>();

            IList<AttentionPlace> attentionPlaces = new List<AttentionPlace>();
            AttentionPlace _AttentionPlace1 = new AttentionPlace(1, true, "0");
            AttentionPlace _AttentionPlace2 = new AttentionPlace(2, true, "0");
            attentionPlaces.Add(_AttentionPlace1);
            attentionPlaces.Add(_AttentionPlace2);
            Office office1 = new Office("OFI-1", _OfficeQueue1, attentionPlaces);

            //Objeto de prueba oficina2
            ConcurrentQueue<TimedQueueItem<string>> _OfficeQueue2 = new ConcurrentQueue<TimedQueueItem<string>>();

            IList<AttentionPlace> attentionPlaces2 = new List<AttentionPlace>();
            AttentionPlace _AttentionPlace3 = new AttentionPlace(3, true, "0");
            AttentionPlace _AttentionPlace4 = new AttentionPlace(4, true, "0");
            attentionPlaces2.Add(_AttentionPlace3);
            attentionPlaces2.Add(_AttentionPlace4);
            Office office2 = new Office("OFI-2", _OfficeQueue2, attentionPlaces2);

            //agrego ambos objetos al diccionario.
            this._Offices.Add(office1.Identificator,office1);
            this._Offices.Add(office2.Identificator,office2);

        }

        public void Add(Office office)
        {
          
            if (office.Identificator == null)
            {
                throw new ArgumentNullException(); 
            }

            this._Offices.Add(office.Identificator, office);

        }

        public void Update(Office office){

            if (office.Identificator != null)
            {
                this._Offices[office.Identificator] = office;
            }
            throw new KeyNotFoundException();
        }
        public void Delete(string id)
        {
            this._Offices.Remove(id);
        }

        public Office GetOffice(string identificator)
        {
            Office office;

            if (this._Offices.TryGetValue(identificator, out office))
            {
                return office;
            }
            
            return null;
           
        }

        public IList<Office> GetAll()
        {

            return this._Offices.Values.ToList();

        }
    }
}
