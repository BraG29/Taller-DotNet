using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quality_Management.Model
{
    public class Procedure
    {

        [Required]
        [Key]
        public long Id { get; init; }
        // [Column("office")]
        [Required]
        [ForeignKey("office")]
        public Office Office { get; set; }
        [Column("place_number")]
        public long PlaceNumber { get; set; }
        [Column("procedure_start")]
        public DateTime ProcedureStart { get; set; }// = DateTime.Now;
        [Column("procedure_end")] 
        public DateTime ProcedureEnd { get; set; }

        [Column("wait_time")]
        public string WaitTime { get; set; }

        public Procedure()
        {
            
        }

        public Procedure(long id, Office office, long placeNumber, DateTime procedureStart, string waitTime)
        {
            Id = id;
            Office = office;
            PlaceNumber = placeNumber;
            ProcedureStart = procedureStart;
            WaitTime = waitTime;
        }
        
    }
}
