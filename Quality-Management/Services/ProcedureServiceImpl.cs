using Microsoft.EntityFrameworkCore;
using Quality_Management.DTO;
using Quality_Management.Model;
using System.Runtime.CompilerServices;

namespace Quality_Management.Services
{
    public class ProcedureServiceImpl : IProcedureService
    {
        private readonly IProcedureRepository _procedureRepository;

        public ProcedureServiceImpl(IProcedureRepository procedureRepository)
        {
            _procedureRepository = procedureRepository;
        }

        public async Task<long> CreateProcedure(ProcedureDTO procedureDTO)
        {

            if (procedureDTO == null)
            {
                throw new ArgumentNullException($"Objeto vacio");
            }

            if (procedureDTO.OfficeId == null)
            {
                throw new ArgumentNullException($"Identificador de oficina vacio");
            }
            try
            {
                Procedure procedure = new Procedure(0, procedureDTO.OfficeId, procedureDTO.PlaceNumber, procedureDTO.ProcedureStart);

                //devolver id generado
                return await _procedureRepository.Save(procedure);
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.ToString());
            }
           
        }

        public void DeleteProcedure(string procedureId)
        {
            throw new NotImplementedException();
        }

        public IList<ProcedureDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public ProcedureDTO GetProcedure(string procedurId)
        {
            throw new NotImplementedException();
        }
    }
}
