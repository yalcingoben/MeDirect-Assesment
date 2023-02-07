using ExchangeTrader.App.Abstractions.Exchange.Exceptions;
using ExchangeTrader.Integration.Fixer.Models;
using Moq;
using System.Text.Json;

namespace ExchangeTrader.Integration.Fixer.Tests
{
    public class FixerExchangeServiceTest
    {
        private readonly Mock<IHttpClientFactory> httpClientFactory;
        private readonly JsonSerializerOptions _options;

        public FixerExchangeServiceTest()
        {
            httpClientFactory = new Mock<IHttpClientFactory>();
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        [Fact]
        public async void GetRates_Should_Get_When_Base_Curreny_Is_Given()
        {
            //Arrange
            var baseCurrency = "EUR";
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"latest?base={baseCurrency}");

            var latestResponse = new LatestResponse()
            {
                Success = true,
                Base = "EUR",
                Date = DateTime.Now,
                Rates = new Dictionary<string, double>
              {
                  { "TRY", 20.38 },
                  { "USD", 1.08 }
              }
            };

            var mockHttpClient = new Mock<HttpClient>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), CancellationToken.None)).ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(latestResponse, _options))
            });
            //Act
            var fixerRateService = new FixerExchangeService(httpClientFactory.Object);
            var result = await fixerRateService.GetRates(baseCurrency, CancellationToken.None);
            //Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Rates.Count());
        }
        [Fact]
        public async void GetRates_Should_Throw_IntegrationFaultException_When_Response_Is_Not_Serialize()
        {
            //Arrange
            var baseCurrency = "EUR";
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"latest?base={baseCurrency}");

            var latestResponse = new LatestResponse()
            {
                Success = false,
                Base = "EUR",
                Date = DateTime.Now,
                Rates = new Dictionary<string, double>
              {
                  { "TRY", 20.38 },
                  { "USD", 1.08 }
              }
            };

            var mockHttpClient = new Mock<HttpClient>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), CancellationToken.None)).ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(JsonSerializer.Serialize(latestResponse, _options))
            });
            //Act
            var fixerRateService = new FixerExchangeService(httpClientFactory.Object);
            var exception = await Assert.ThrowsAsync<IntegrationFaultException>(() => fixerRateService.GetRates(baseCurrency, CancellationToken.None));
            //Assert            
            Assert.Equal("The error occurred while exchange rates were deserializing.", exception.Message);
            Assert.Equal("Fixer", exception.Provider);
        }

        [Fact]
        public async void GetRates_Should_Throw_IntegrationFaultException_When_HttpResponse_Is_Failed()
        {
            //Arrange
            var baseCurrency = "EUR";
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"latest?base={baseCurrency}");

            var mockHttpClient = new Mock<HttpClient>();
            httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(mockHttpClient.Object);
            mockHttpClient.Setup(x => x.SendAsync(It.IsAny<HttpRequestMessage>(), CancellationToken.None)).ReturnsAsync(() => new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.NotFound,
                Content = new StringContent("")
            });
            //Act
            var fixerRateService = new FixerExchangeService(httpClientFactory.Object);
            var exception = await Assert.ThrowsAsync<IntegrationFaultException>(() => fixerRateService.GetRates(baseCurrency, CancellationToken.None));
            //Assert            
            Assert.Equal("The error occurred while exchange rates were getting.", exception.Message);
            Assert.Equal("Fixer", exception.Provider);
        }
    }
}