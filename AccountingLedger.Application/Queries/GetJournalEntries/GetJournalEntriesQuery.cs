using MediatR;
using AccountingLedger.Domain.Dtos;
namespace AccountingLedger.Application.Queries.GetJournalEntries
{
    public record GetJournalEntriesQuery() : IRequest<List<JournalEntryDto>>;
    public record JournalLineDto(int AccountId, string AccountName, decimal Debit, decimal Credit);

}
