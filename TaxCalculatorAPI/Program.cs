using Application.TaxCalculation;
using FluentValidation;
using Infrastructure;
using Infrastructure.Data;
using TaxCalculatorAPI.EndPoints;
using TaxCalculatorAPI.Request;
using TaxCalculatorAPI.Validations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<IValidator<TaxRecordRequest>, TaxRecordValidator>();
builder.Services.AddScoped<ITaxCalculationFactory, TaxCalculationFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await app.InitialiseDatabaseAsync();

    app.UseSwagger();
    app.UseSwaggerUI();
}

using IServiceScope serviceScope = app.Services.CreateScope();
using ApplicationDBContext dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

app.UseHttpsRedirection();

app.AddPostalCodeEndPoints(dbContext);
app.AddTaxRecordEndPoints(dbContext);

app.Run();