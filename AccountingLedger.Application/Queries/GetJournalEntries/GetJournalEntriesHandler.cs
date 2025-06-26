using AccountingLedger.Infrastructure.Services;
using MediatR;
using AccountingLedger.Domain.Dtos;
namespace AccountingLedger.Application.Queries.GetJournalEntries
{
    public class GetJournalEntriesHandler : IRequestHandler<GetJournalEntriesQuery, List<JournalEntryDto>>
    {
        private readonly JournalRepository _journalRepo;

        public GetJournalEntriesHandler(JournalRepository journalRepo) =>
            _journalRepo = journalRepo;

        public async Task<List<JournalEntryDto>> Handle(GetJournalEntriesQuery request, CancellationToken cancellationToken) =>
            await _journalRepo.GetJournalEntriesAsync();
    }
}
