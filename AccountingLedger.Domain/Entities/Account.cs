namespace AccountingLedger.Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
