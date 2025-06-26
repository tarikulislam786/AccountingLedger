namespace AccountingLedger.Domain.Dtos
{
    public class JournalLineDto
    {
        public int AccountId { get; set; }
        public string AccountName {  get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
