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

                Procedure procedure = new Procedure(0, _officeRepository.FindById(procedureDTO.OfficeId), procedureDTO.PlaceNumber, procedureDTO.ProcedureStart,
                    procedureDTO.WaitTime);

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
        public async Task<long> ProceduresAmount(string officeId)
        {

            if (officeId == null)
            {
                throw new ArgumentNullException($"identificador de oficina vacio");
            }

            try
            {
                IList<Procedure> procedures = await _procedureRepository.FindProceduresByOffice(officeId);

                Console.WriteLine($"Cantidad de tramites: {procedures.Count} por oficina: {officeId}");

                return procedures.Count;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new DbUpdateConcurrencyException(ex.ToString());
            }
            
        }

        public async Task<string> ProceduresAverageTime(string officeId)
        {

            if (officeId == null)
            {
                throw new ArgumentNullException($"Identificador vacio");
            }

            IList<Procedure> procedures = await _procedureRepository.FindProceduresByOffice(officeId);
            
            if(procedures == null)
            {
                throw new ArgumentNullException($"No se encontró la oficina");
            }

            TimeSpan total = TimeSpan.Zero;

            int validDate = 0; //contador para fechas validas

            foreach (Procedure procedure in procedures)
            {
                DateTime start = procedure.ProcedureStart;
                DateTime end = procedure.ProcedureEnd;

                if (start != DateTime.MinValue || end != DateTime.MinValue)
                {
                    TimeSpan duration = end - start;

                    total += duration;
                    validDate++;
                } 
            }

            //Realizo un promedio utilizando los ticks del total y divido entre las fechas validas,
            //omitiendo fechas invalidas, ej: valores por defecto.
            TimeSpan average = new TimeSpan(total.Ticks / validDate);
            
            string averageString = average.ToString(@"hh\:mm\:ss");

            return averageString;
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
                procedure.PlaceNumber, procedure.ProcedureStart, procedure.ProcedureEnd,
                procedure.WaitTime);


            return procedureDTO;
        }

        public async Task<string> ProceduresAverageWaitTime(string officeId)
        {

            IList<Procedure> procedures = await _procedureRepository.FindProceduresByOffice(officeId);

            TimeSpan total = TimeSpan.Zero;

            //Aplicar controles ya que fechas por defecto me causan medidas negativas
            foreach (Procedure procedure in procedures)
            {

                if(TimeSpan.TryParse(procedure.WaitTime, out TimeSpan durationWaitTime)){

                    total += durationWaitTime;

                }

            }

            TimeSpan average = new TimeSpan(total.Ticks / procedures.Count);

            string averageString = average.ToString(@"hh\:mm\:ss");

            return averageString;
        }
    }

}


