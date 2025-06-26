using AccountingLedger.Application.Commands.CreateAccount;
using AccountingLedger.Infrastructure.Services;
using AccountingLedger.Application.Validators;
using FluentValidation;
using AccountingLedger.Application.Commands.CreateJournalEntry;
using AccountingLedger.Application.Common.Behaviors;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// ✅ Register MediatR handlers from the Application assembly
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(CreateAccountHandler).Assembly);
});


// DI for repositories
builder.Services.AddScoped<AccountRepository>();

builder.Services.AddScoped<JournalRepository>();

/// Register validators from Application project
//builder.Services.AddValidatorsFromAssembly(typeof(CreateAccountValidator).Assembly);
//builder.Services.AddValidatorsFromAssembly(typeof(CreateJournalEntryValidator).Assembly);

//builder.Services.AddScoped<IValidator<CreateAccountCommand>, CreateAccountValidator>();
//builder.Services.AddScoped<IValidator<CreateJournalEntryCommand>, CreateJournalEntryValidator>();

// Register All Validators
builder.Services.AddValidatorsFromAssembly(typeof(AccountingLedger.Application.Commands.CreateAccount.CreateAccountHandler).Assembly);

// Register Validation Behavior
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


// ✅ Other services
builder.Services.AddControllers();


builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Register your exception middleware first
app.UseMiddleware<AccountingLedger.WebApi.Middleware.ExceptionHandlingMiddleware>();


app.UseHttpsRedirection();



app.UseAuthorization();

app.MapControllers();

app.Run();
