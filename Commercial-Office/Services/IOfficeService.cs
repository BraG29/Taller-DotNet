using Commercial_Office.DTO;

namespace Commercial_Office.Services
{
    public interface IOfficeService
    {
        /// <summary>
        /// Funcion para crear una oficina que recibe un OfficeDTO y lo transforma a objeto de dominio,
        /// y lo envia al repositorio para ser guardado en un Diccionario
        /// </summary>
        public void CreateOffice(OfficeDTO office);

        /// <summary>
        /// Funcion para actualizar una oficina, en este caso solo puestos de atencion al cliente
        /// </summary>
        /// <param name="officeId"></param>
        /// <param name="places"></param>
        public void UpdateOffice(string officeId, IList<AttentionPlaceDTO> places);

        /// <summary>
        /// Funcion para eliminar una oficina utilizando su identificador.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteOffice(string id);
        
        /// <summary>
        /// Funcion para obtener una oficina utilizando su identificador.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OfficeDTO GetOffice(string id);

        /// <summary>
        /// Funcion para obtener todas las oficinas registradas en el sistema
        /// </summary>
        /// <returns></returns>
        public IList<OfficeDTO> GetAll();

        /// <summary>
        /// Funcion para registrar a un usuario e ingresarlo en una queue.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="officeId"></param>
        public void RegisterUser(string  userId, string officeId);

        /// <summary>
        /// Funcion para que un usuario (operador) libere un puesto que se encuentra ocupado.
        /// </summary>
        /// <param name="officeId"></param>
        /// <param name="placeNumber"></param>
        public Task ReleasePosition(string officeId, long placeNumber);

        /// <summary>
        /// Funcion para que un usuario (operador) solicite que se le asigne el siguiente
        /// usuario en la queue.
        /// </summary>
        /// <param name="officeId"></param>
        /// <param name="placeNumber"></param>
        public Task CallNextUser(string officeId, long placeNumber);
    }
}
