using Quality_Management.DTO;
using Quality_Management.Model;
using System.Diagnostics.Eventing.Reader;

namespace Quality_Management.Services
{
    public interface IProcedureService
    {
        /// <summary>
        /// Funcion para crear un tramitea que recibe un ProcedureDTO y lo transforma a objeto de dominio,
        /// y lo envia al repositorio para ser guardado en una bd
        /// </summary>
        public Task<long> CreateProcedure(ProcedureDTO procedure);

        /// <summary>
        /// Funcion para finalizar un tramite ya creado. La misma se encarga de cargar la fecha de finalizacion de un tramite
        /// obtenida desde el servicio de Commercial Office
        /// </summary>
        /// <param name="procedureId"> Identificador del tramite </param>
        /// <param name="procedureFinishTime"> Fecha y hora en la que finaliza el tramite </param>
        /// <returns></returns>
        public Task EndProcedure(long procedureId, DateTime procedureFinishTime);

        /// <summary>
        /// Funcion que obtiene datos necesarios para crear metricas retroactivas.
        /// </summary>
        /// <param name="officeId"> Identificador de la oficina de la cual se quiere obtener el dato </param>
        /// <param name="range"> Rango de tiempo semana, mes o año</param>
        /// <param name="interval"> Intervalo en el que se organizan los datos, en semanas, dias o meses</param>
        /// <returns> Devuelve el promedio de tiempo, en el intervalo seleccionado  </returns>
        public Task<IList<ProcedureMetricsDTO>> RetroactiveMetricsData(string officeId, TimeRange range);

        /// <summary>
        /// Funcion para obtener un procedimiento utilizando su identificador.
        /// </summary>
        /// <param name="procedureId"></param>
        /// <returns> Un procedimiento especifico de tipo ProcedureDTO</returns>
        public Task<ProcedureDTO> GetProcedure(long procedureId);
    }

}
