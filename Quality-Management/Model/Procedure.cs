namespace Quality_Management.Model
{
    public class Procedure
    {

        public string ProcedureId { get; set; }

        public string OfficeId { get; set; }

        public long PlaceNumber { get; set; }

        public DateTime ProcedureStart { get; set; }

        public DateTime ProcedureEnd { get; set; }


        public Procedure(string procedureId, string officeId, long placeNumber, DateTime procedureStart, DateTime procedureEnd)
        {
            ProcedureId = procedureId;
            OfficeId = officeId;
            PlaceNumber = placeNumber;
            ProcedureStart = procedureStart;
            ProcedureEnd = procedureEnd;
        }
    }
}
