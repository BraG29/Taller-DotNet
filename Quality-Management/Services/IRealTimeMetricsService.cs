using Quality_Management.Model;

namespace Quality_Management.Services;

public interface IRealTimeMetricsService
{
    public void ClientEnterTheQueue(string officeId);
    public void ClientLeavesTheQueue(string officeId);
    public int ClientsInQueue(string officeId);
    public int FreePositions(string officeId);
}