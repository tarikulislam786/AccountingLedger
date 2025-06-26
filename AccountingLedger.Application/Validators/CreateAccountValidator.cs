using AccountingLedger.Application.Commands.CreateAccount;
using FluentValidation;

namespace AccountingLedger.Application.Validators
{
    public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountValidator()
        {
            RuleFor(a => a.Name)
                .NotEmpty().WithMessage("Account Name is required.")
                .MinimumLength(2).WithMessage("Account Name must be at least 2 characters.")
                .MaximumLength(100).WithMessage("Account Name must not exceed 100 characters.");

            RuleFor(a => a.Type)
                .NotEmpty().WithMessage("Account Type is required.")
                .Must(type => new[] { "Asset", "Liability", "Equity", "Revenue", "Expense" }.Contains(type))
                .WithMessage("Account Type must be one of the following: Asset, Liability, Equity, Revenue, Expense.");
        }
    }

    public class CreateAccountDto
    {
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
    }
}
