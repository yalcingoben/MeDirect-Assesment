using ExchangeTrader.App.Abstractions.Services;
using ExchangeTrader.App.Configurations;
using ExchangeTrader.App.Dtos;
using ExchangeTrader.Integration.Fixer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExchangeTrader.Integration.Fixer
{
    public class FixerExchangeService : IExchangeService
    {
        private readonly IHttpClientFactory httpClientFactory;        
        private readonly JsonSerializerOptions _options;

        public FixerExchangeService(
            IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<ExchangeRateDto> GetRates(string baseCurrency)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"latest?base={baseCurrency}");

            var client = httpClientFactory.CreateClient("exchangeRateApi");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var latestResponse = await JsonSerializer.DeserializeAsync<LatestResponse>(responseStream, _options);
                if (latestResponse != null && latestResponse.Success)
                {
                    return new ExchangeRateDto
                    {
                        Date = latestResponse.Date,
                        Rates = latestResponse.Rates.Select(rate => new RateDto { Code = rate.Key, Value = rate.Value }).ToList()
                    };
                }
            }
            return new ExchangeRateDto();
        }

        public async Task<IEnumerable<SymbolDto>> GetSymbols()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "symbols");

            var client = httpClientFactory.CreateClient("exchangeRateApi");
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var symbolsResponse = await JsonSerializer.DeserializeAsync<SymbolsResponse>(responseStream, _options);
                if(symbolsResponse != null && symbolsResponse.Success) 
                { 
                    return symbolsResponse.Symbols.Select(symbol => new SymbolDto { Code = symbol.Key, Name = symbol.Value });
                }
            }
            return Enumerable.Empty<SymbolDto>();
        }
    }
}
