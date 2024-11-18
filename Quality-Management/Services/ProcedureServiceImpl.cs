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


        //Funcion general de obtencion de metricas
        public async Task<IList<ProcedureMetricsDTO>> RetroactiveMetricsData(string officeId, TimeRange range)
        {

            if (officeId == null)
            {
                throw new ArgumentNullException($"Identificador vacio");
            }

            //Dependiendo del caso obtengo datos desde 7 dias atras respecto a la fecha actual, hasta 1 año.
            DateTime startDate = range switch //Fecha desde la cual comienza la busqueda
            {
                TimeRange.LastWeek => DateTime.Now.AddDays(-7).Date,
                TimeRange.LastMonth => DateTime.Now.AddMonths(-1),
                TimeRange.LastYear => DateTime.Now.AddYears(-1),
                _ => DateTime.MinValue //Este valor funcionaria como una consulta historica al no marcar limite de rango
            };

            DateTime endDate = DateTime.Now.Date;//Fecha actual del sistema

            IList<Procedure> procedures = await _procedureRepository.FindProceduresByOfficeAndDate(officeId, startDate, endDate);
            
            if(procedures == null)
            {
                throw new ArgumentNullException($"No se encontró la oficina");
            }

            var interval = GetGroupingIntervalByTimeRange(range);

            //Agrupo los datos de las fechas en diario, semanal y mensual
            var proceduresGroup = procedures.GroupBy(procedure => interval switch {
                GroupingInterval.Daily => procedure.ProcedureStart.Date,
                GroupingInterval.Weekly => StartOfWeek(procedure.ProcedureStart),
                GroupingInterval.Monthly => new DateTime(procedure.ProcedureStart.Year, procedure.ProcedureStart.Month, 1),
                _ => procedure.ProcedureStart.Date
            });

            var metricsList = new List<ProcedureMetricsDTO>();

            foreach (var procedureGrouped in proceduresGroup)
            {

                TimeSpan totalWaitTime = TimeSpan.Zero;
                TimeSpan total = TimeSpan.Zero;

                int validDate = 0; //contador para fechas validas


                foreach (var procedure in procedureGrouped)
                {

                    DateTime start = procedure.ProcedureStart;
                    DateTime end = procedure.ProcedureEnd;

                    if (start != DateTime.MinValue || end != DateTime.MinValue)
                    {
                        TimeSpan duration = end - start;

                        total += duration;
                        validDate++;
                    }

                    // Sumatoria de tiempos de espera
                    if (TimeSpan.TryParse(procedure.WaitTime, out TimeSpan waitTime))
                    {
                        totalWaitTime += waitTime;
                    }

                }

                //Realizo un promedio utilizando los ticks del total y divido entre las fechas validas,
                //omitiendo fechas invalidas, ej: valores por defecto.
                TimeSpan averageTime = validDate > 0 ? new TimeSpan(total.Ticks / validDate) : TimeSpan.Zero;

                //Promedio del tiempo de espera
                TimeSpan averageWaitTime = validDate > 0 ? new TimeSpan(totalWaitTime.Ticks / validDate) : TimeSpan.Zero;

                metricsList.Add(new ProcedureMetricsDTO
                {
                    IntervalDate = procedureGrouped.Key,
                    ProcedureAverageWaitTime = averageWaitTime.ToString(@"hh\:mm\:ss"),
                    ProcedureAverageDurationTime = averageTime.ToString(@"hh\:mm\:ss"),
                    ProcedureCount = validDate
                });
            }

            return metricsList;
        }


        public async Task<ProcedureDTO> GetProcedure(long procedureId)
        {

            var procedure = await _procedureRepository.FindById(procedureId);

            if (procedure == null)
            {
                throw new ArgumentNullException($"El tramite no existe");
            }

            //id   office, place, start, end.

            ProcedureDTO procedureDTO = new ProcedureDTO(procedure.Id, procedure.Office.OfficeId,
                procedure.PlaceNumber, procedure.ProcedureStart, procedure.ProcedureEnd,
                procedure.WaitTime);

            return procedureDTO;
        }



        //Funciones auxiliares:

        /// <summary>
        /// Esta funcion obtiene la fecha de inicio de la semana a la que pertenece una fecha. Para agrupar datos semanalmente.
        /// </summary>
        /// <param name="date"> Fecha de la que se quiere obtener la semana</param>
        /// <returns> La fecha del dia lunes de esa semana</returns>
        public static DateTime StartOfWeek(DateTime date)
        {
            DayOfWeek startOfWeek = DayOfWeek.Monday;

            int diff = (7 + (date.DayOfWeek - startOfWeek)) % 7;
            return date.AddDays(-1 * diff).Date;
        }
        
        /// <summary>
        /// Esta funcion se asegura de aplicar el siguiente formato de intervalos de acuerado al rango de informacion
        /// de la siguiente manera: Ultima semana se agrupa en dias, Ultimo mes se agrupa en semanas y 
        /// ultimo año se agrupa en meses.
        /// </summary>
        /// <param name="range"> Recibe el rango de tiempo </param>
        /// <returns>Devuelve el intervalo en el que se agrupan los datos</returns>
        public static GroupingInterval GetGroupingIntervalByTimeRange(TimeRange range)
        {
            return range switch
            {
                TimeRange.LastWeek => GroupingInterval.Daily,
                TimeRange.LastMonth => GroupingInterval.Weekly,
                TimeRange.LastYear => GroupingInterval.Monthly,
                _ => GroupingInterval.Daily
            };
        }


    }

}


