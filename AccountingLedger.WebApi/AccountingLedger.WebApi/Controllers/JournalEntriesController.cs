using AccountingLedger.Application.Commands.CreateJournalEntry;
using AccountingLedger.Application.Queries.GetJournalEntries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountingLedger.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JournalEntriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public JournalEntriesController(IMediator mediator) => _mediator = mediator;

        #region ========== Get Journal Entries ==========
        [HttpGet]
        public async Task<IActionResult> GetJournalEntries() =>
            Ok(await _mediator.Send(new GetJournalEntriesQuery()));
        #endregion

        #region ========== Create Journal Entries ==========
        [HttpPost]
        public async Task<IActionResult> CreateJournalEntry([FromBody] CreateJournalEntryCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(new { error = result.Error });
            }

            // Build the response
            var response = new
            {
                message = "Journal entry created successfully.",
                journalEntry = new
                {
                    id = result.Value,
                    date = command.Date,
                    description = command.Description,
                    lines = command.Lines.Select(line => new
                    {
                        accountId = line.AccountId,
                        debit = line.Debit,
                        credit = line.Credit
                    }).ToList()
                }
            };

            return CreatedAtAction(nameof(GetJournalEntries), new { id = result.Value }, response);
        }
        #endregion
    }
}
