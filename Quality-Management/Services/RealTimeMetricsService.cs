using Quality_Management.Infraestructure;
using Quality_Management.Model;

namespace Quality_Management.Services;

public class RealTimeMetricsService : IRealTimeMetricsService
{
    private readonly IDictionary<String, Office> _offices;

    public RealTimeMetricsService(IOfficeRepository officeRepository)
    {
        _offices = officeRepository.FindAll().Result
            .Select(office => new Office
            {
                OfficeId = office.OfficeId, 
                PositionsAmount = office.PositionsAmount, 
                FreePositions = office.PositionsAmount
                
            })
            .ToDictionary(office => office.OfficeId);
    }
    
    public void ClientEnterTheQueue(string officeId)
    { 
        _offices[officeId].ClientsInQueue += 1;
    }

    public void ClientLeavesTheQueue(string officeId)
    {
        _offices[officeId].ClientsInQueue -= 1;
        _offices[officeId].FreePositions -= 1;
    }

    public int ClientsInQueue(string officeId)
    {
        return _offices[officeId].ClientsInQueue;
    }

    public int FreePositions(string officeId)
    {
        return _offices[officeId].FreePositions;
    }
    
}