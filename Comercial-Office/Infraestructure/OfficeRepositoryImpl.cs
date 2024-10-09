﻿using Comercial_Office.Model;

namespace Comercial_Office.Infraestructure
{
    public class OfficeRepositoryImpl : IOfficeRepository
    {

        private IDictionary<string, Office> _Offices;

        public OfficeRepositoryImpl() { 
                
            _Offices = new Dictionary<string, Office>();

            Office office1 = new Office("OFI-1", null, null);
            Office office2 = new Office("OFI-2", null, null);

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
            if(id != null)
            {
                this._Offices.Remove(id);
            }

            throw new KeyNotFoundException();
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