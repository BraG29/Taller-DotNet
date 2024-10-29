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
        public string OfficeId { get; set; }
        [Column("place_number")]
        public long PlaceNumber { get; set; }
        [Column("procedure_start")] 
        public DateTime ProcedureStart { get; set; }
        [Column("procedure_end")] 
        public DateTime? ProcedureEnd { get; set; }

        public Procedure()
        {
            
        }
        
        public Procedure(long id, string officeId, long placeNumber, DateTime procedureStart)
        {
            Id = id;
            OfficeId = officeId;
            PlaceNumber = placeNumber;
            ProcedureStart = procedureStart;
        }
        
    }
}
