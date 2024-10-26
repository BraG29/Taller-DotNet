namespace Commercial_Office.Model
{
    public class AttentionPlace
    {
        public ulong Number { get; set; }
        
        public bool IsAvailable { get; set; }

        public string ProcessId { get; set; }

        public AttentionPlace() { }

        public AttentionPlace(ulong number, bool isAvailable, string processId)
        {
            Number = number;
            IsAvailable = isAvailable;
            ProcessId = processId;
        }
    }
}
