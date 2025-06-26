using AccountingLedger.Application.Commands.CreateAccount;
using AccountingLedger.Application.Common;
using AccountingLedger.Infrastructure.Services;
using MediatR;

namespace AccountingLedger.Application.Commands.CreateJournalEntry
{
    public class CreateJournalEntryHandler : IRequestHandler<CreateJournalEntryCommand, Result<int>>
    {
        private readonly JournalRepository _journalRepository;

        public CreateJournalEntryHandler(JournalRepository journalRepository)
        {
            _journalRepository = journalRepository ?? throw new ArgumentNullException(nameof(journalRepository));
        }

        public async Task<Result<int>> Handle(CreateJournalEntryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var id = await _journalRepository.CreateJournalEntryAsync(request.Date, request.Description, request.Lines);
                return Result<int>.Success(id);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error creating journal entry: {ex.Message}");
            }
        }
    }
}
