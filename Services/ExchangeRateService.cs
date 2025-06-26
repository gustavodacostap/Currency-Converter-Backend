using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using CurrencyConverter.Backend.Models;

namespace CurrencyConverter.Backend.Services
{
    public class ExchangeRateService
    {
        private readonly HttpClient _httpClient;

        public ExchangeRateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ConversionResult?> ConvertCurrencyAsync(string from, string to, decimal amount)
        {
            try
            {
                string url = $"https://api.exchangerate.host/convert?from={from}&to={to}&amount={amount}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Erro na requisição externa: {response.StatusCode}");
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var document = JsonDocument.Parse(json);

                var resultElement = document.RootElement.GetProperty("result");
                var rateElement = document.RootElement.GetProperty("info").GetProperty("rate");

                return new ConversionResult
                {
                    From = from,
                    To = to,
                    Amount = amount,
                    Rate = rateElement.GetDecimal(),
                    Result = resultElement.GetDecimal()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
                return null;
            }
        }
    }
}
