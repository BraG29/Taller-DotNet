namespace Comercial_Office.DTO
{
    public class OfficeDTO
    {
        public string? Identificator { get; set; }
        public IList<AttentionPlaceDTO>? Posts { get; set; }

        public OfficeDTO(string identificator, IList<AttentionPlaceDTO> posts)
        {
            this.Identificator = identificator;
            this.Posts = posts;
        }

        public OfficeDTO() { }

    }
}
