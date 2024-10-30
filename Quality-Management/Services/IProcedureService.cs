using Quality_Management.DTO;

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
        /// Funcion para eliminar un tramite utilizando su identificador.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteProcedure(string procedureId);

        /// <summary>
        /// Funcion para obtener un procedimiento utilizando su identificador.
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Un procedimiento especifico de tipo ProcedureDTO</returns>
        public ProcedureDTO GetProcedure(string procedurId);

        /// <summary>
        /// Funcion para obtener todos los tramites registrados en el sistema
        /// </summary>
        /// <returns> Una lista de tramites</returns>
        public IList<ProcedureDTO> GetAll();



    }
}
