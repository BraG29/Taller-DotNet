namespace Comercial_Office.DTO
{
    public class OfficeDTO
    {
        public string? Identificator { get; set; }
        //esto capaz que deja de ser una lista
        public IList<AttentionPlaceDTO>? Posts { get; set; }

        //persona de la queue
        //public string person {get; set;}
        public OfficeDTO(string identificator, IList<AttentionPlaceDTO> posts)
        {
            this.Identificator = identificator;
            this.Posts = posts;
        }

        public OfficeDTO() { }

    }
}
