using Microsoft.AspNetCore.SignalR;
using Quality_Management.Hubs;
using Quality_Management.Infraestructure;
using Quality_Management.Model;

namespace Quality_Management.Services;

public class RealTimeMetricsService : IRealTimeMetricsService
{
    private readonly IDictionary<String, Office> _offices;
    private readonly IOfficeRepository _officeRepository;
    private readonly IHubContext<QualityManagementHub> _hub;

    public RealTimeMetricsService(IOfficeRepository officeRepository, IHubContext<QualityManagementHub> hub)
    {
        _officeRepository = officeRepository;
        _hub = hub;
        _offices = _officeRepository.FindAll().Result
            .Select(office => new Office
            {
                OfficeId = office.OfficeId, 
                PositionsAmount = office.PositionsAmount, 
                FreePositions = office.PositionsAmount
                
            })
            .ToDictionary(office => office.OfficeId);
    }
    
    public int[] ClientEnterTheQueue(string officeId)
    { 
        return [++ _offices[officeId].ClientsInQueue];
    }

    public int[] ClientLeavesTheQueue(string officeId)
    {
        return [-- _offices[officeId].ClientsInQueue, -- _offices[officeId].FreePositions];
    }

    public int[] PositionReleased(string officeId)
    {
        return [++ _offices[officeId].FreePositions];
    }

    public async void SendMetric(IRealTimeMetricsService.ChangeMetricStatus operation, string officeId)
    {
        if (!ExistsOffice(officeId)) throw new ArgumentException($"No existe la oficina '{officeId}'");
        
        await _hub.Clients.All.SendAsync(operation.Method.Name, officeId, operation(officeId));
        
    }

    private bool ExistsOffice(string officeId)
    {
        if (_offices.ContainsKey(officeId)) return true;
        
        Office? office = _officeRepository.FindById(officeId);

        if (office != null)
        {
            _offices.Add(officeId, new Office
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