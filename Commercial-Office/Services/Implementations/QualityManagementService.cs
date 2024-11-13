using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System.Net.Http;

namespace Commercial_Office.Services.Implementations
{
    public class QualityManagementService(HttpClient httpClient)
    {

        /// <summary>
        /// Se comunica con el servicio de Quality Management para iniciar un tramite
        /// guarda en el contexto el identificador del tramite actual
        /// </summary>
        /// <param name="officeId"> Identificador de la oficina </param>
        /// <param name="placeNumber"> Numero de puesto de la oficina donde ocurre el tramite</param>
        /// <param name="processStartDate">Fecha en la que se comienza el tramite</param>
        public async Task<string> StartProcedure(string officeId, long placeNumber, DateTime processStartDate)
        {

            var data = new
            {
                OfficeId = officeId,
                PlaceNumber = placeNumber,
                procedureStart = processStartDate
            };

            var response = await httpClient
            .PostAsJsonAsync("quality-management-api/startProcedure", data);

            string procedureId = await response.Content.ReadAsStringAsync();

            return procedureId;
            //TODO: Ver este tema con los gurises
            //memoryCache.Set("CurrentProcedureId", procedureId, TimeSpan.FromMinutes(5));
        }

        /// <summary>
        /// Se comunica con el servicio de Quality Management para finalizar un tramite
        /// </summary>
        /// <param name="procedureEnd"> Fecha en la que se terminó el tramite </param>
        /// <param name="procedureId"> Identificador del tramite </param>
        public async Task FinishProcedure(long procedureId, DateTime procedureEnd)
        {

            await httpClient.PutAsJsonAsync($"quality-management-api/finishProcedure/{procedureId}", procedureEnd);
            /*
            if (memoryCache.TryGetValue("CurrentProcedureId", out var id))
            {
                long procedureId = Convert.ToInt64(id);
                Console.WriteLine("\n\n hice la request \n\n ");
            }
            else
            {
                throw new InvalidOperationException("$No hay un procedimiento en curso.");
            }*/

        }

        public async Task CallClientRegistration(string officeId)
        {
            var response = await httpClient.GetAsync($"quality-management-api/client-registration/{officeId}");
            if (!response.IsSuccessStatusCode)
            {
                Console.Write($"Ha habido un error al enviar metrica a QM: {response.Content.ReadAsStringAsync()}");
            }
        }

    }
}
