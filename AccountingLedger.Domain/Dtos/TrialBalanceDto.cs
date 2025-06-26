namespace AccountingLedger.Domain.Dtos
{
    public class TrialBalanceDto
    {
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
    }
}
