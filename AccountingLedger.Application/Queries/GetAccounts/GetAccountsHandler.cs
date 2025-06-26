using AccountingLedger.Infrastructure.Services;
using MediatR;

namespace AccountingLedger.Application.Queries.GetAccounts
{
    public class GetAccountsHandler : IRequestHandler<GetAccountsQuery, List<AccountDto>>
    {
        private readonly AccountRepository _accountRepo;

        public GetAccountsHandler(AccountRepository accountRepo) =>
            _accountRepo = accountRepo;

        public async Task<List<AccountDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken) =>
            (await _accountRepo.GetAccountsAsync())
                .Select(a => new AccountDto(a.Id, a.Name, a.Type))
                .ToList();
    }
}
