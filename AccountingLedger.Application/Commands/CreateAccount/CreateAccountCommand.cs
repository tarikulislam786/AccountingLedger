using AccountingLedger.Application.Common;
using MediatR;

namespace AccountingLedger.Application.Commands.CreateAccount
{
    public record CreateAccountCommand(string Name, string Type) : IRequest<Result<int>>;
}
