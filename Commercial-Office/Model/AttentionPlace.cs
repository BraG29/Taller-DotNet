using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

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

        [NotMapped] public long ProcedureId { get; set; } = 0;

        // public string OfficeId { get; set; }
        //
        // [ForeignKey("OfficeId")]
        // [JsonIgnore]
        // public Office office { get; set; }

        public AttentionPlace() { }

        public AttentionPlace(long id,  long number, bool isAvailable)
        {
            this.AttentionPlaceId = id;
            Number = number;
            IsAvailable = isAvailable;
            // OfficeId = officeId;
        }
    }
}
