using AccountingLedger.Application.Common;
using AccountingLedger.Infrastructure.Services;
using MediatR;

namespace AccountingLedger.Application.Commands.CreateAccount
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, Result<int>>
    {
        private readonly AccountRepository _accountRepo;

        public CreateAccountHandler(AccountRepository accountRepository)
        {
            _accountRepo = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        public async Task<Result<int>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            // Suppose we have some business rule error
            if (string.IsNullOrEmpty(request.Name))
            {
                return Result<int>.Failure("Account Name is required.");
            }

            var accountId = await _accountRepo.CreateAccountAsync(request.Name, request.Type);
            return Result<int>.Success(accountId);
        }

    }
}
