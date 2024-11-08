using Quality_Management.DTO;
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
        /// Funcion que obtiene la cantidad de procedimientos realizados por una oficina
        /// </summary>
        /// <param name="officeId"> Identificador de la oficina de la cual se quiere obtener el dato</param>
        /// <returns> La cantidad de tramites realizados </returns>
        public Task<long> ProceduresAmount(string officeId);

        /// <summary>
        /// Funcion que obtiene el promedio de tiempo que demoran los tramites.
        /// </summary>
        /// <param name="officeId"> Identificador de la oficina de la cual se quiere obtener el dato </param>
        /// <returns> Devuelve el promedio de tiempo </returns>
        public Task<string> ProceduresAverageTime(string officeId);

        /// <summary>
        /// Funcion que obtiene el promedio del tiempo de espera de los usuarios en la cola
        /// </summary>
        /// <param name="officeId">Identificador de la oficina de la cual se quiere obtener el dato </param>
        /// <returns >Devuelve el promedio de tiempo de espera </returns>
        public Task<string> ProceduresAverageWaitTime(string officeId);



        /// <summary>
        /// Funcion para obtener un procedimiento utilizando su identificador.
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Un procedimiento especifico de tipo ProcedureDTO</returns>
        public Task<ProcedureDTO> GetProcedure(long procedurId);
    }

}

/*
      /// <summary>
      /// Funcion para eliminar un tramite utilizando su identificador.
      /// </summary>
      /// <param name="id"></param>
      public Task DeleteProcedure(long procedureId);

      /// <summary>
      /// Funcion para obtener un procedimiento utilizando su identificador.
      /// </summary>
      /// <param name="id"></param>
      /// <returns> Un procedimiento especifico de tipo ProcedureDTO</returns>
      public Task<ProcedureDTO> GetProcedure(long procedurId);

      /// <summary>
      /// Funcion para obtener todos los tramites registrados en el sistema
      /// </summary>
      /// <returns> Una lista de tramites</returns>
      public Task<IList<ProcedureDTO>> GetAll();*/