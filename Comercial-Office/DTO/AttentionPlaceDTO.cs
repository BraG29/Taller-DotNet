namespace Comercial_Office.DTO
{
    public class AttentionPlaceDTO
    {
        public long? Number { get; set; }

        public bool? Available { get; set; }

        public AttentionPlaceDTO() { }

        public AttentionPlaceDTO(long? number, bool? available) {
            this.Number = number;
            this.Available = available;
        }

    }

    
}
