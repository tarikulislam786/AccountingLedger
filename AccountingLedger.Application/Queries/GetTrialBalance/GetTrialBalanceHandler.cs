using AccountingLedger.Domain.Dtos;
using AccountingLedger.Infrastructure.Services;
using MediatR;

namespace AccountingLedger.Application.Queries.GetTrialBalance
{
    public class GetTrialBalanceHandler : IRequestHandler<GetTrialBalanceQuery, List<TrialBalanceDto>>
    {
        private readonly JournalRepository _journalRepo;

        public GetTrialBalanceHandler(JournalRepository journalRepo) =>
            _journalRepo = journalRepo;

        public async Task<List<TrialBalanceDto>> Handle(GetTrialBalanceQuery request, CancellationToken cancellationToken) =>
            await _journalRepo.GetTrialBalanceAsync();
    }
}
