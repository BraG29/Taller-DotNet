namespace API_Gateway_Client.DTOs
{
    public class ClientOfficeDTO
    {
        public string? Identificator { get; set; }
        public IList<AttentionPlaceDTO>? AttentionPlaces { get; set; }

        public ClientOfficeDTO(string identificator, IList<AttentionPlaceDTO> attentionPlaces)
        {
            this.Identificator = identificator;
            this.AttentionPlaces = attentionPlaces;
        }

        public ClientOfficeDTO() { }

        public void FromOfficeDTO(String id){
            this.Identificator = id;
        }
    }

    
}
