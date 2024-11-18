using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Commercial_Office.Model
{
    public class AttentionPlace
    {
        [Key]
        [Column("PlaceId")]
        public long AttentionPlaceId { get; set; }

        [Column("place_number")]
        public long Number { get; set; }

        [Column("State")]
        public bool IsAvailable { get; set; }

        public long ProcedureId { get; set; }

        public string OfficeId { get; set; }

        [ForeignKey("OfficeId")]
        public Office office { get; set; }

        public AttentionPlace() { }

        public AttentionPlace(long id,  long number, bool isAvailable, long procedureId, string officeId)
        {
            this.AttentionPlaceId = id;
            Number = number;
            IsAvailable = isAvailable;
            ProcedureId = procedureId;
            OfficeId = officeId;
        }
    }
}
