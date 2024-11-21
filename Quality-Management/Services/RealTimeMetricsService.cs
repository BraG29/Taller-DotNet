using Microsoft.AspNetCore.SignalR;
using Quality_Management.Hubs;
using Quality_Management.Infraestructure;
using Quality_Management.Model;
using Microsoft.AspNetCore.SignalR.Client;

namespace Quality_Management.Services
{
    public static class HubConnectionExtensions
    {

        //the function in question that does the configuration
        public static IHubConnectionBuilder WithUrl(this IHubConnectionBuilder builder, string url,
            IHttpMessageHandlerFactory clientFactory)
        {
            return builder.WithUrl(url,
                options => { options.HttpMessageHandlerFactory = _ => clientFactory.CreateHandler(); });
        }
    }

    public class RealTimeMetricsService : IRealTimeMetricsService
    {

        private readonly IRedisServer _redisServer;
        private readonly IOfficeRepository _officeRepository;
        private readonly HubConnection _hub;

        public RealTimeMetricsService(IOfficeRepository officeRepository, IRedisServer redisServer,
            IHttpMessageHandlerFactory msgFactory)
        {
            _officeRepository = officeRepository;
            _redisServer = redisServer;

            var hubConnection = new HubConnectionBuilder();

            //now we just use the static configuration function that David Fowler wrote above
            HubConnectionExtensions.WithUrl(hubConnection, "http://api-gateway/connection", msgFactory);

            //and we build the connection
            _hub = hubConnection.Build();
        }

        public int[] ClientEnterTheQueue(string officeId)
        {
            var office = _redisServer.GetValueAsync(officeId).Result;
            int[] metrics = [++office.ClientsInQueue];
            _redisServer.SetValueAsync(office.OfficeId, office);
            return metrics;
        }

        public int[] ClientLeavesTheQueue(string officeId)
        {
            var office = _redisServer.GetValueAsync(officeId).Result;
            int[] metrics = [--office.ClientsInQueue, --office.FreePositions];
            _redisServer.SetValueAsync(office.OfficeId, office);
            return metrics;
        }

        public int[] PositionReleased(string officeId)
        {
            var office = _redisServer.GetValueAsync(officeId).Result;
            int[] metrics = [++office.FreePositions];
            _redisServer.SetValueAsync(office.OfficeId, office);
            return metrics;
        }

        public async Task SendMetric(IRealTimeMetricsService.ChangeMetricStatus operation, string officeId)
        {
            if (!ExistsOffice(officeId)) throw new ArgumentException($"No existe la oficina '{officeId}'");

            if (_hub.State != HubConnectionState.Connected)
            {
                await _hub.StartAsync(); //try to reconenct
                await _hub.InvokeAsync(operation.Method.Name, officeId, operation(officeId)); //we send the data
            }
            else
            {
                await _hub.InvokeAsync(operation.Method.Name, officeId, operation(officeId));
            }

        }

        private bool ExistsOffice(string officeId)
        {
            if (_redisServer.GetValueAsync(officeId).Result != null) return true;

            Office? office = _officeRepository.FindById(officeId);

            if (office != null)
            {
                _redisServer.SetValueAsync(officeId, new Office
                {
                    OfficeId = office.OfficeId,
                    PositionsAmount = office.PositionsAmount,
                    FreePositions = office.PositionsAmount
                });

                return true;
            }

            return false;

        }
    }
}

