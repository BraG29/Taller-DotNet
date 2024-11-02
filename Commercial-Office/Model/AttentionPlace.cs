namespace Commercial_Office.Model
{
    public class AttentionPlace
    {
        public ulong Number { get; set; }
        
        public bool IsAvailable { get; set; }

        public long ProcedureId { get; set; }

        public AttentionPlace() { }

        public AttentionPlace(ulong number, bool isAvailable, long procedureId)
        {
            Number = number;
            IsAvailable = isAvailable;
            ProcedureId = procedureId;
        }
    }
}
