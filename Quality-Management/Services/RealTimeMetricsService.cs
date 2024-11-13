using Microsoft.AspNetCore.SignalR;
using Quality_Management.Hubs;
using Quality_Management.Infraestructure;
using Quality_Management.Model;

namespace Quality_Management.Services;

public class RealTimeMetricsService : IRealTimeMetricsService
{
    private readonly IRedisServer _redisServer;
    private readonly IOfficeRepository _officeRepository;
    private readonly IHubContext<QualityManagementHub> _hub;

    public RealTimeMetricsService(IOfficeRepository officeRepository, IRedisServer redisServer, IHubContext<QualityManagementHub> hub)
    {
        _officeRepository = officeRepository;
        _redisServer = redisServer;
        _hub = hub;
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
        int[] metrics = [ --office.ClientsInQueue, --office.FreePositions ];
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
        
        await _hub.Clients.All.SendAsync(operation.Method.Name, officeId, operation(officeId));
        
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