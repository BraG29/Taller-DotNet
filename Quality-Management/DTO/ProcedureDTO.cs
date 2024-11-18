using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Quality_Management.DTO
{
    public class ProcedureDTO
    {

        public long Id { get; set; }

        public string OfficeId { get; set; }

        public long PlaceNumber { get; set; }

        public DateTime ProcedureStart { get; set; }

        public DateTime ProcedureEnd { get; set; }

        public string WaitTime { get; set; }

        public ProcedureDTO() { }

        public ProcedureDTO(long id, string officeId, long placeNumber, DateTime procedureStart, DateTime procedureEnd, string waitTime)
        {
            Id = id;
            OfficeId = officeId;
            PlaceNumber = placeNumber;
            ProcedureStart = procedureStart;
            ProcedureEnd = procedureEnd;
            WaitTime = waitTime;
        }
    }
}
