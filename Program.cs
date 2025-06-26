var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<CurrencyConverter.Backend.Services.ExchangeRateService>();

var app = builder.Build();

app.MapControllers();

app.Run();
