using Quality_Management.Model;

namespace Quality_Management.Services;

public interface IRealTimeMetricsService
{
    public delegate int[] ChangeMetricStatus(string officeId);
    public int[] ClientEnterTheQueue(string officeId);
    public int[] ClientLeavesTheQueue(string officeId);
    public int[] PositionReleased(string officeId);
    public void SendMetric(IRealTimeMetricsService.ChangeMetricStatus operation, string officeId);
}