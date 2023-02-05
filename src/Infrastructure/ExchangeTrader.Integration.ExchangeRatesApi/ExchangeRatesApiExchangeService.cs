using ExchangeTrader.App.Abstractions.Exchange;
using ExchangeTrader.App.Abstractions.Exchange.Dtos;
using ExchangeTrader.App.Abstractions.Exchange.Exceptions;
using ExchangeTrader.Integration.ExchangeRatesApi.Models;
using System.Text.Json;

namespace ExchangeTrader.Integration.ExchangeRatesApi
{
    public class ExchangeRatesApiExchangeService : IExchangeService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly JsonSerializerOptions _options;

        public ExchangeRatesApiExchangeService(
            IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;            
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<ExchangeRate> GetRates(string baseCurrency, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"latest?base={baseCurrency}");

            var client = httpClientFactory.CreateClient("exchangeRateApi");
            var response = await client.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var latestResponse = await JsonSerializer.DeserializeAsync<LatestResponse>(responseStream, _options, cancellationToken);
                if (latestResponse != null && latestResponse.Success)
                {
                    return new ExchangeRate
                    {
                        Date = latestResponse.Date,
                        Rates = latestResponse.Rates.Select(rate => new Rate { Code = rate.Key, Value = rate.Value }).ToList()
                    };
                }
                throw new IntegrationFaultException("The error occurred while exchange rates were deserializing.", (int)response.StatusCode, "ExchangeRatesApi");
            }
            throw new IntegrationFaultException("The error occurred while exchange rates were getting.", (int)response.StatusCode, "ExchangeRatesApi");
        }        
    }
}
