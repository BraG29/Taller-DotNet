using Microsoft.EntityFrameworkCore;
using Quality_Management.DTO;
using Quality_Management.Model;
using System.Data;
using System.Runtime.CompilerServices;

namespace Quality_Management.Services
{
    public class ProcedureServiceImpl : IProcedureService
    {
        private readonly IProcedureRepository _procedureRepository;
        private readonly IOfficeRepository _officeRepository;

        public ProcedureServiceImpl(IProcedureRepository procedureRepository, IOfficeRepository officeRepository)
        {
            _procedureRepository = procedureRepository;
            _officeRepository = officeRepository;
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

            if (!_officeRepository.ExistsById(procedureDTO.OfficeId))
            {
                throw new ArgumentException($"No existe la oficina {procedureDTO.OfficeId}");
            }
            
            try
            {
                Procedure procedure = new Procedure(0, _officeRepository.FindById(procedureDTO.OfficeId), procedureDTO.PlaceNumber, procedureDTO.ProcedureStart);

                //devolver id generado
                return await _procedureRepository.Save(procedure);
            }
            catch (DbUpdateException ex)
            {
                throw new DbUpdateException(ex.ToString());
            }

        }

        public async Task EndProcedure(long procedureId, DateTime procedureFinishTime)
        {
            try
            {

                if (procedureId <= 0)
                {
                    throw new ArgumentException("El ID del procedimiento debe ser mayor que cero.");
                }

                var procedure = await _procedureRepository.FindById(procedureId);

                if (procedure == null)
                {
                    throw new ArgumentNullException($"El tramite no existe");
                }

                procedure.ProcedureEnd = procedureFinishTime;
                await _procedureRepository.Update(procedure);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine("DbUpdateConcurrencyException: " + ex.ToString());
                throw; 
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("DbUpdateException: " + ex.ToString());
                throw;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine("Error inesperado: " + ex.ToString());
                throw;
            }
        }

        //cant tramites x oficina
        //tiempo promedio tramites x oficina
        public Task<long> ProceduresAmount(string officeId)
        {
            throw new NotImplementedException();
        }

        public Task<double> ProceduresAverageTime(string officeId)
        {
            throw new NotImplementedException();
        }


        public async Task<ProcedureDTO> GetProcedure(long procedureId)
        {

            var procedure = await _procedureRepository.FindById(procedureId);

            if (procedure == null)
            {
                throw new ArgumentNullException($"El tramite no existe");
            }
            
            //id ,  office, place, start, end.
            ProcedureDTO procedureDTO = new ProcedureDTO(procedure.Id, procedure.Office.OfficeId,
                procedure.PlaceNumber, procedure.ProcedureStart, procedure.ProcedureEnd);

            return procedureDTO;
        }

    }

}

/*
public async Task DeleteProcedure(long procedureId)
{

    try
    {
        var procedure = await _procedureRepository.FindById(procedureId);

        if (procedure == null)
        {
            throw new ArgumentNullException($"El tramite no existe");
        }

        await _procedureRepository.Delete(procedure);
    }
    catch (DbUpdateException ex)
    {
        throw new DbUpdateException(ex.ToString());
    }

}

}

public async Task<IList<ProcedureDTO>> GetAll()
{
    try
    {
        var procedureList = await _procedureRepository.FindAll();

        ProcedureDTO procedureDTO;
        IList<ProcedureDTO> procedureDTOList = new List<ProcedureDTO>();

        foreach (Procedure procedure in procedureList)
        {

            procedureDTO = new ProcedureDTO(procedure.Id, procedure.OfficeId,
                procedure.PlaceNumber, procedure.ProcedureStart, procedure.ProcedureEnd);

            procedureDTOList.Add(procedureDTO);
        }

        return procedureDTOList;
    }
    catch(Exception ex) {

        throw new Exception(ex.ToString());

    }
}

public async Task<ProcedureDTO> GetProcedure(long procedureId)
{

    var procedure = await _procedureRepository.FindById(procedureId);

    if (procedure == null)
    {
        throw new ArgumentNullException($"El tramite no existe");
    }

    //id ,  office, place, start, end.
    ProcedureDTO procedureDTO = new ProcedureDTO(procedure.Id, procedure.OfficeId,
        procedure.PlaceNumber, procedure.ProcedureStart, procedure.ProcedureEnd);

    return procedureDTO;
}
*/

