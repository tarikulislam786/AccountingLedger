namespace AccountingLedger.Domain.Entities
{
    public class JournalEntry
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public List<JournalEntryLine> Lines { get; set; } = new();
    }
}
