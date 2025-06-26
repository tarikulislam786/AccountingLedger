using AccountingLedger.Domain.Dtos;
using MediatR;

namespace AccountingLedger.Application.Queries.GetTrialBalance
{
    public record GetTrialBalanceQuery() : IRequest<List<TrialBalanceDto>>;
}
