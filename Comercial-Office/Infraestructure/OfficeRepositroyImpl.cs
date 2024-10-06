using Comercial_Office.Model;

namespace Comercial_Office.Infraestructure
{
    public class OfficeRepositroyImpl : IOfficeRepository
    {

        private IDictionary<string, Office> _Offices;

        public OfficeRepositroyImpl() { 
                
            _Offices = new Dictionary<string, Office>();

            Office office1 = new Office("OFI-1", null, null);
            Office office2 = new Office("OFI-2", null, null);

            this._Offices.Add(office1.Identificator,office1);
            this._Offices.Add(office2.Identificator,office2);

        }

        public void Add(Office office)
        {
            this._Offices.Add(office.Identificator,office);
        }

        public void Update(Office office){
            this._Offices[office.Identificator] = office;
        }
        public void Delete(string id)
        {
            this._Offices.Remove(id);
        }

        public Office GetOffice(string identificator)
        {

            if (this._Offices != null)
            {
                Office office = this._Offices[identificator];
                return office;
            }
            
            return null;
        }
        public IList<Office> GetAll()
        {
            if (this._Offices != null)
            {
                
                return this._Offices.Values.ToList();
            }

            return null;
        }
    }
}
