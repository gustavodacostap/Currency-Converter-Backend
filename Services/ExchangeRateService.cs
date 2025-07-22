using System.Text.Json;
using CurrencyConverter.Backend.Models;
using CurrencyConverter.Backend.Configurations;

namespace CurrencyConverter.Backend.Services
{
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;

        public ExchangeRateService(HttpClient httpClient, CurrencyApiOptions options)
        {
            _httpClient = httpClient;
            _baseUrl = options.BaseUrl;
            _apiKey = options.ApiKey;
        }

        public async Task<ConversionResult?> ConvertCurrencyAsync(string from, string to, decimal amount)
        {
            try
            {
                string url = $"{_baseUrl}/latest?apikey={_apiKey}&base_currency={from}&currencies={to}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro HTTP externo: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

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
