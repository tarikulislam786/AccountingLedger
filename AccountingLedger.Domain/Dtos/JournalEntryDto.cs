namespace AccountingLedger.Domain.Dtos
{
    public class JournalEntryDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public List<JournalLineDto> Lines { get; set; } = new List<JournalLineDto>();
    }
}
