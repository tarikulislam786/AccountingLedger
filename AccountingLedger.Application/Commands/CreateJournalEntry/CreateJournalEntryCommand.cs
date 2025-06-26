using AccountingLedger.Application.Common;
using AccountingLedger.Domain.Dtos;
using MediatR;

namespace AccountingLedger.Application.Commands.CreateJournalEntry
{
    public record CreateJournalEntryCommand(DateTime Date, string Description, List<JournalLineDto> Lines) : IRequest<Result<int>>;
}
