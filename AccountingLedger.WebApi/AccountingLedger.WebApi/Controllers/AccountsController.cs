using AccountingLedger.Application.Commands.CreateAccount;
using AccountingLedger.Application.Queries.GetAccounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountingLedger.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountsController(IMediator mediator) => _mediator = mediator;


        #region ========== Get Accounts ==========
        [HttpGet]
        public async Task<IActionResult> GetAccounts() =>
            Ok(await _mediator.Send(new GetAccountsQuery()));

        #endregion

        #region ========== Create Account ==========
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
        {
            var result = await _mediator.Send(command);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Errors = result.Errors });
            }
            // Build the response object
            var response = new
            {
                message = "Account created successfully.",
                account = new
                {
                    id = result.Value,
                    name = command.Name,
                    type = command.Type
                }
            };
            return CreatedAtAction(nameof(GetAccounts), new { id = result.Value }, response);
        }
        #endregion
    }
}
