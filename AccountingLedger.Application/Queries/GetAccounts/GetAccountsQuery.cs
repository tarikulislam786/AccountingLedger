using MediatR;

namespace AccountingLedger.Application.Queries.GetAccounts
{
    public record GetAccountsQuery() : IRequest<List<AccountDto>>;
    public record AccountDto(int Id, string Name, string Type);
}
