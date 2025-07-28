using ExchangeRateOffers.Api.Application;
using ExchangeRateOffers.Api.Application.Interfaces.Services;
using ExchangeRateOffers.Api.Application.Services;
using ExchangeRateOffers.Api.Infrastructure;
using ExchangeRateOffers.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ICompareExchangeRatesService, CompareExchangeRatesService>();

builder.Services.AddTransient<GlobalExceptioHandlerMiddleware>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<GlobalExceptioHandlerMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
