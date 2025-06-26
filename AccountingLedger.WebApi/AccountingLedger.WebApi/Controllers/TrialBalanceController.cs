using AccountingLedger.Application.Queries.GetTrialBalance;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AccountingLedger.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrialBalanceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TrialBalanceController(IMediator mediator) => _mediator = mediator;

        #region ========== Get TrialBalance ==========
        [HttpGet]
        public async Task<IActionResult> GetTrialBalance() =>
            Ok(await _mediator.Send(new GetTrialBalanceQuery()));
        #endregion
    }
}
