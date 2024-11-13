using Commercial_Office.Model;
using System.Collections.Concurrent;

namespace Commercial_Office.Services.Interfaces
{
    public interface IOfficeQueueService
    {

        /// <summary>
        /// Funcion encargada de: devolver la cola de la oficina si existe, o crearla de no existir.
        /// La misma se crea y guarda en un Diccionario en memoria.
        /// </summary>
        /// <param name="officeId"> El identificador de la oficina</param>
        /// <returns> Devuelve la cola de la oficina solicitada </returns>
        public ConcurrentQueue<TimedQueueItem<string>> GetQueueOrCreateQueue(string officeId);
    }
}
