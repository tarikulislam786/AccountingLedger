using FluentValidation;
using System.Text.Json;

namespace AccountingLedger.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";

                var errors = ex.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

                var errorResponse = new
                {
                    title = "Validation error",
                    status = 400,
                    errors
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
            catch (Exception ex)
            {
                // Optional: You can also log the error here
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    title = "Internal Server Error",
                    status = 500,
                    detail = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }
    }
}
