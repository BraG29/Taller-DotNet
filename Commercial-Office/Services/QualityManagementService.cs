﻿namespace Commercial_Office.Services
{
    public class QualityManagementService(HttpClient httpClient)
    {
        
        /// <summary>
        /// Se comunica con el servicio de Quality Management para iniciar un tramite
        /// </summary>
        /// <param name="officeId"> Identificador de la oficina </param>
        /// <param name="placeNumber"> Numero de puesto de la oficina donde ocurre el tramite</param>
        /// <param name="processStartDate">Fecha en la que se comienza el tramite</param>
        /// <returns> Devuelve un identificador del tramite </returns>
        public async Task<string> StartProcedure(string officeId, long placeNumber, DateTime processStartDate)
        {

            var data = new
            {
                OfficeId = officeId,
                PlaceNumber = placeNumber,
                procedureStart = processStartDate
            };

            Console.WriteLine(data.procedureStart);

            var response = await httpClient
            .PostAsJsonAsync("quality-management-api/startProcedure", data);

            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Se comunica con el servicio de Quality Management para finalizar un tramite
        /// </summary>
        /// <param name="procedureId"> Identificador del tramite </param>
        /// <param name="procedureFinishDate"> Fecha en la que se terminó el tramite </param>
        public async Task FinishProcedure(long procedureId, DateTime procedureFinishDate)
        {
            await httpClient.PutAsJsonAsync("quality-management-api/finishProcedure/"+procedureId , procedureFinishDate);
        }

     
    }
}
