using ExchangeTrader.App.Abstractions.Exchange;
using ExchangeTrader.App.Abstractions.Exchange.Dtos;
using ExchangeTrader.App.Abstractions.Exchange.Exceptions;
using ExchangeTrader.Integration.Fixer.Models;
using System.Text.Json;

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

        public async Task<ExchangeRate> GetRates(string baseCurrency, CancellationToken cancellationToken)
        {
            //TODO: remove before production
            //return new ExchangeRate
            //{
            //    Date = DateTime.Now,
            //    Rates = new List<Rate>
            //    {
            //        new Rate{ Code = "TRY", Value = 20.37 },
            //        new Rate{ Code = "USD", Value = 1.08 }
            //    }
            //};

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"latest1?base={baseCurrency}");

            var client = httpClientFactory.CreateClient("fixer");
            var response = await client.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var latestResponse = await JsonSerializer.DeserializeAsync<LatestResponse>(responseStream, _options);
                if (latestResponse != null && latestResponse.Success)
                {
                    return new ExchangeRate
                    {
                        Date = latestResponse.Date,
                        Rates = latestResponse.Rates.Select(rate => new Rate { Code = rate.Key, Value = rate.Value }).ToList()
                    };
                }
                throw new IntegrationFaultException("The error occurred while exchange rates were deserializing.", (int)response.StatusCode, "Fixer");
            }
            throw new IntegrationFaultException("The error occurred while exchange rates were getting.", (int)response.StatusCode, "Fixer");
        }        
    }
}
