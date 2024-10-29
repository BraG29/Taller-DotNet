using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quality_Management.Model
{
    public class Procedure
    {

        [Required]
        [Key]
        public long Id { get; init; }
        [Column("office")]
        [Required, StringLength(8)]
        public string OfficeId { get; set; } = string.Empty;
        [Column("place_number")]
        public long PlaceNumber { get; init; }
        [Column("procedure_start")] 
        public DateTime ProcedureStart { get; init; } = DateTime.Now;
        [Column("procedure_end")]
        public DateTime? ProcedureEnd { get; set; }

        // public Procedure()
        // {
        //     
        // }
        //
        // public Procedure(long procedureId, string officeId, long placeNumber, DateTime procedureStart, DateTime procedureEnd)
        // {
        //     Id = procedureId;
        //     OfficeId = officeId;
        //     PlaceNumber = placeNumber;
        //     ProcedureStart = procedureStart;
        //     ProcedureEnd = procedureEnd;
        // }
    }
}
