using CurrencyConverter.Backend.Services;
using CurrencyConverter.Backend.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var baseUrl = Environment.GetEnvironmentVariable("CURRENCY_API_BASE_URL") 
              ?? "https://api.freecurrencyapi.com/v1";

var apiKey = Environment.GetEnvironmentVariable("CURRENCY_API_KEY") 
             ?? throw new Exception("API key não definida.");

builder.Services.AddSingleton(new CurrencyApiOptions
{
    BaseUrl = baseUrl,
    ApiKey = apiKey
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient<ExchangeRateService>();var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.MapControllers();

app.Run();
