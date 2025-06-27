using CurrencyConverter.Backend.Services;
using CurrencyConverter.Backend.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<CurrencyApiOptions>(builder.Configuration.GetSection("CurrencyApi"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<ExchangeRateService>();var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
