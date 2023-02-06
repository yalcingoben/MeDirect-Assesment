using ExchangeTrader.App.Abstractions.Caching;
using ExchangeTrader.App.Abstractions.Exchange;
using ExchangeTrader.App.Abstractions.Exchange.Configurations;
using ExchangeTrader.App.Abstractions.Exchange.Dtos;
using ExchangeTrader.App.Features.Rate.Query;
using Moq;

namespace ExchangeTrader.App.Tests.Features.Rate.Query
{
    public class GetRatesQueryHandlerTest
    {
        private readonly Mock<IExchangeService> _exchangeService;
        private readonly Mock<ICacheService> _cacheService;
        private readonly ExchangeCurrencyConfiguration _exchangeCurrencyConfiguration;

        public GetRatesQueryHandlerTest() 
        { 
            _exchangeService = new Mock<IExchangeService>();
            _exchangeCurrencyConfiguration = new ExchangeCurrencyConfiguration()
            {
                BaseSystemCurrency = "EUR",
                CurrencyRateTimeout = new TimeSpan(0,0,30),
                ExchangeIntegrationProvider = Abstractions.Exchange.Enums.ExchangeIntegrationProvider.Fixer
            };
            _cacheService = new Mock<ICacheService>();
        }

        [Fact]
        public async void Handle_Should_ThrowArgumentNullException_When_Query_Response_Is_Null()
        {
            //Arrange
            var request = new GetRatesQuery
            {
                BaseCurrency = "EUR"
            };

            var exchangeRate = new ExchangeRate
            {
                Date = DateTime.Now,
                Rates = new List<Abstractions.Exchange.Dtos.Rate>
                {
                    new Abstractions.Exchange.Dtos.Rate{ Code = "USD", Value = 1.08 },
                    new Abstractions.Exchange.Dtos.Rate{ Code = "TRY", Value = 20.38 }
                }
            };

            _exchangeService.Setup(x => x.GetRates(It.IsAny<string>(), default)).ReturnsAsync(()=> exchangeRate);
            _cacheService.Setup(x => x.GetOrAddAsync<ExchangeRate>(
                It.IsAny<string>(),
                It.IsAny<Func<Task<ExchangeRate>>>(),
                It.IsAny<TimeSpan?>(), 
                default)).ReturnsAsync(()=> null);

            //Act
            var handler = new GetRatesQueryHandler(_exchangeService.Object, _cacheService.Object, _exchangeCurrencyConfiguration);
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(request, CancellationToken.None));
            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'result')", exception.Message);
        }
        [Fact]
        public async void Handle_Should_ThrowArgumentNullException_When_Query_Response_Rates_Are_Null()
        {
            //Arrange
            var request = new GetRatesQuery
            {
                BaseCurrency = "EUR"
            };

            var exchangeRate = new ExchangeRate
            {
                Date = DateTime.Now,
                Rates = new List<Abstractions.Exchange.Dtos.Rate>
                {
                    new Abstractions.Exchange.Dtos.Rate{ Code = "USD", Value = 1.08 },
                    new Abstractions.Exchange.Dtos.Rate{ Code = "TRY", Value = 20.38 }
                }
            };

            _exchangeService.Setup(x => x.GetRates(It.IsAny<string>(), default)).ReturnsAsync(() => exchangeRate);
            _cacheService.Setup(x => x.GetOrAddAsync<ExchangeRate>(
                It.IsAny<string>(),
                It.IsAny<Func<Task<ExchangeRate>>>(),
                It.IsAny<TimeSpan?>(),
                default)).ReturnsAsync(() => new ExchangeRate { Rates = null });

            //Act
            var handler = new GetRatesQueryHandler(_exchangeService.Object, _cacheService.Object, _exchangeCurrencyConfiguration);
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(request, CancellationToken.None));
            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'Rates')", exception.Message);
        }

        [Fact]
        public async void Handle_Should_Return_Query_Response_When_Rates_Are_Not_Null()
        {
            //Arrange
            var request = new GetRatesQuery
            {
                BaseCurrency = "EUR"
            };

            var exchangeRate = new ExchangeRate
            {
                Date = DateTime.Now,
                Rates = new List<Abstractions.Exchange.Dtos.Rate>
                {
                    new Abstractions.Exchange.Dtos.Rate{ Code = "USD", Value = 1.08 },
                    new Abstractions.Exchange.Dtos.Rate{ Code = "TRY", Value = 20.38 }
                }
            };

            _exchangeService.Setup(x => x.GetRates(It.IsAny<string>(), default)).ReturnsAsync(() => exchangeRate);
            _cacheService.Setup(x => x.GetOrAddAsync<ExchangeRate>(
                It.IsAny<string>(),
                It.IsAny<Func<Task<ExchangeRate>>>(),
                It.IsAny<TimeSpan?>(),
                default)).ReturnsAsync(() => exchangeRate);

            //Act
            var handler = new GetRatesQueryHandler(_exchangeService.Object, _cacheService.Object, _exchangeCurrencyConfiguration);
            var result = await handler.Handle(request, CancellationToken.None);
            //Assert
            Assert.Equal(2, result.Rates.Count());
        }
    }
}
