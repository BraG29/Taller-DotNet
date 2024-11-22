namespace API_Gateway_Client.DTOs
{
    public class ProcedureMetricsDTO
    {
        public DateTime IntervalDate { get; set; }

        public string ProcedureAverageWaitTime { get; set; }
        public string ProcedureAverageDurationTime { get; set; }
        public long ProcedureCount { get; set; }


        public ProcedureMetricsDTO() { }

        public ProcedureMetricsDTO(DateTime intervalDate, string procedureAverageWaitTime, string procedureAverageDurationTime, long procedureCount)
        {
            IntervalDate = intervalDate;
            ProcedureAverageWaitTime = procedureAverageWaitTime;
            ProcedureAverageDurationTime = procedureAverageDurationTime;
            ProcedureCount = procedureCount;
        }
    }
}