using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CurrencyConverter.Backend.Services;

namespace CurrencyConverter.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConverterController : ControllerBase
    {
        private readonly ExchangeRateService _exchangeRateService;

        public ConverterController(ExchangeRateService exchangeRateService)
        {
            _exchangeRateService = exchangeRateService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string from, [FromQuery] string to, [FromQuery] decimal amount)
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to) || amount <= 0)
                return BadRequest("Parâmetros inválidos");

            var result = await _exchangeRateService.ConvertCurrencyAsync(from.ToUpper(), to.ToUpper(), amount);

            if (result == null)
                return StatusCode(500, "Erro ao obter cotação");

            return Ok(result);
        }
    }
}
