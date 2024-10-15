namespace Commercial_Office.Model
{
    public class AttentionPlace
    {
        public long Number { get; set; }
        
        public bool IsAvailable { get; set; }



        public AttentionPlace() { }

        public AttentionPlace(long number, bool isAvailable)
        {
            Number = number;
            IsAvailable = isAvailable;
        }
    }
}
