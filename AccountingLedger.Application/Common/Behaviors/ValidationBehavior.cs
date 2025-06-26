using FluentValidation;
using MediatR;

namespace AccountingLedger.Application.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) =>
            _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var results = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = results.SelectMany(r => r.Errors).Where(f => f is not null).ToList();

                if (failures.Any())
                {
                    //throw new ValidationException(failures);
                    var errorMessages = failures.Select(f => f.ErrorMessage).ToList();

                    // You can create an error Result<TResponse> if your handlers use Result<T>:
                    var resultType = typeof(TResponse).GetGenericArguments().First();
                    var errorResult = typeof(Result<>).MakeGenericType(resultType)
                        .GetMethod("Failure", new[] { typeof(string) })!
                        .Invoke(null, new object[] { string.Join(", ", errorMessages) });

                    return (TResponse)errorResult!;
                }
            }

            return await next();
        }
    }
}
