using System.Text.Json;
using CurrencyConverter.Backend.Models;
using CurrencyConverter.Backend.Configurations;
using Microsoft.Extensions.Options;

namespace CurrencyConverter.Backend.Services
{
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly CurrencyApiOptions _options;

        public ExchangeRateService(HttpClient httpClient, IOptions<CurrencyApiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<ConversionResult?> ConvertCurrencyAsync(string from, string to, decimal amount)
        {
            try
            {
                // 1. Busca a taxa base_currency -> to
                string url = $"{_options.BaseUrl}/latest?apikey={_options.ApiKey}&base_currency={from}&currencies={to}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro HTTP externo: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);

                var root = doc.RootElement;

                // Pega a taxa de conversão dentro de "data"
                if (!root.TryGetProperty("data", out var dataElement))
                    return null;

                if (!dataElement.TryGetProperty(to, out var rateElement))
                    return null;

                decimal rate = rateElement.GetDecimal();

                return new ConversionResult
                {
                    From = from,
                    To = to,
                    Amount = amount,
                    Rate = rate,
                    Result = amount * rate
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro no ExchangeRateService: " + ex.Message);
                return null;
            }
        }
    }
}
