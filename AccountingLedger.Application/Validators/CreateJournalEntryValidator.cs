using FluentValidation;
using AccountingLedger.Domain.Dtos;
using AccountingLedger.Application.Commands.CreateJournalEntry;
namespace AccountingLedger.Application.Validators
{
    public class CreateJournalEntryValidator : AbstractValidator<CreateJournalEntryCommand>
    {
        public CreateJournalEntryValidator()
        {
            RuleFor(x => x.Date).NotEmpty().WithMessage("Date is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleForEach(x => x.Lines).ChildRules(line =>
            {
                line.RuleFor(x => x.AccountId).GreaterThan(0).WithMessage("AccountId must be valid.");
                line.RuleFor(x => x.Debit).GreaterThanOrEqualTo(0).WithMessage("Debit must be >= 0.");
                line.RuleFor(x => x.Credit).GreaterThanOrEqualTo(0).WithMessage("Credit must be >= 0.");
            });

            RuleFor(x => x.Lines.Sum(line => line.Debit))
                .Equal(x => x.Lines.Sum(line => line.Credit))
                .WithMessage("Total Debit must equal Total Credit.");
        }
    }

    public class CreateJournalEntryDto
    {
        public DateTime Date { get; set; }
        public string Description { get; set; } = null!;
        public List<JournalLineDto> Lines { get; set; } = new();
    }
}
