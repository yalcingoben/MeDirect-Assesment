using ExchangeTrader.App.Abstractions.Exchange;
using ExchangeTrader.App.Abstractions.Exchange.Configurations;
using ExchangeTrader.App.Abstractions.Exchange.Dtos;
using ExchangeTrader.App.Exceptions;
using ExchangeTrader.App.Features.Rate.Models;
using ExchangeTrader.App.Features.Rate.Query;
using ExchangeTrader.App.Features.Trade.Command;
using ExchangeTrader.App.Features.Trade.Models;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeTrader.App.Tests.Features.Trade.Command
{
    public class ExchangeTradeCommandHandlerTest
    {
        private readonly Mock<IMediator> mediator;
        private readonly ExchangeCurrencyConfiguration exchangeCurrencyConfiguration;
        private readonly Mock<IHttpContextAccessor> httpContextAccessor;
        private readonly Mock<IRateConverterService> rateConverterService;
        private readonly Mock<ILogger<ExchangeTradeCommandHandler>> logger;

        public ExchangeTradeCommandHandlerTest()
        {
            mediator = new Mock<IMediator>();
            exchangeCurrencyConfiguration= new ExchangeCurrencyConfiguration
            {
                BaseSystemCurrency = "EUR"
            };
            httpContextAccessor= new Mock<IHttpContextAccessor>();
            rateConverterService= new Mock<IRateConverterService>();
            logger = new Mock<ILogger<ExchangeTradeCommandHandler>>();
        }

        [Fact]
        public async void Handle_Should_Throw_ArgumentNullException_When_Query_Response_Is_Null()
        {
            //Arrange
            var request = new ExchangeTradeCommand
            {
                Amount = 1,
                Base = "ASD",
                Target = "TRY"
            };

            mediator.Setup(x => x.Send(It.IsAny<GetRatesQuery>(), CancellationToken.None)).ReturnsAsync(() => null);
            var handler = new ExchangeTradeCommandHandler(mediator.Object, 
                exchangeCurrencyConfiguration, 
                httpContextAccessor.Object,
                logger.Object,
                rateConverterService.Object);
            //Act
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => handler.Handle(request, CancellationToken.None));
            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'ratesQueryResponse')", exception.Message);
        }

        [Fact]
        public async void Handle_Should_Throw_CurrencyNotSupportedException_When_Query_Response_Does_Not_Contains__Requested_Base_Currency()
        {
            //Arrange
            var request = new ExchangeTradeCommand
            {
                Amount = 1,
                Base = "ASD",
                Target = "TRY"
            };

            var queryResponse = new GetRatesQueryResponse
            {
                BaseCurrency = "EUR",
                Rates = new List<Dtos.RateDto>
                {
                    new Dtos.RateDto { Code = "USD", Value = 1.08 },
                    new Dtos.RateDto { Code = "TRY", Value = 20.38 }
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<GetRatesQuery>(), CancellationToken.None)).ReturnsAsync(() => queryResponse);
            var handler = new ExchangeTradeCommandHandler(mediator.Object,
                exchangeCurrencyConfiguration,
                httpContextAccessor.Object,
                logger.Object,
                rateConverterService.Object);

            //Act
            var exception = await Assert.ThrowsAsync<CurrencyNotSupportedException>(() => handler.Handle(request, CancellationToken.None));
            //Assert
            Assert.Equal("ASD is not supported currency", exception.Message);
        }
        [Fact]
        public async void Handle_Should_Throw_CurrencyNotSupportedException_When_Query_Response_Does_Not_Contains__Requested_Target_Currency()
        {
            //Arrange
            var request = new ExchangeTradeCommand
            {
                Amount = 1,
                Base = "EUR",
                Target = "ASD"
            };

            var queryResponse = new GetRatesQueryResponse
            {
                BaseCurrency = "EUR",
                Rates = new List<Dtos.RateDto>
                {
                    new Dtos.RateDto { Code = "USD", Value = 1.08 },
                    new Dtos.RateDto { Code = "TRY", Value = 20.38 }
                }
            };

            mediator.Setup(x => x.Send(It.IsAny<GetRatesQuery>(), CancellationToken.None)).ReturnsAsync(() => queryResponse);
            var handler = new ExchangeTradeCommandHandler(mediator.Object,
                exchangeCurrencyConfiguration,
                httpContextAccessor.Object,
                logger.Object,
                rateConverterService.Object);

            //Act
            var exception = await Assert.ThrowsAsync<CurrencyNotSupportedException>(() => handler.Handle(request, CancellationToken.None));
            //Assert
            Assert.Equal("ASD is not supported currency", exception.Message);
        }
        [Fact]
        public async void Handle_Should_Return_Converted_Amount_When_Request_Is_Valid()
        {
            //Arrange
            var request = new ExchangeTradeCommand
            {
                Amount = 1,
                Base = "EUR",
                Target = "TRY"
            };

            var queryResponse = new GetRatesQueryResponse
            {
                BaseCurrency = "EUR",
                Rates = new List<Dtos.RateDto>
                {
                    new Dtos.RateDto { Code = "USD", Value = 1.08 },
                    new Dtos.RateDto { Code = "TRY", Value = 20.38 }
                }
            };

            var mockHttpContext = new Mock<HttpContext>();
            var items = new Dictionary<object, object>
            {
                { "apiKey", "abc" }
            };
            mediator.Setup(x => x.Send(It.IsAny<GetRatesQuery>(), CancellationToken.None)).ReturnsAsync(() => queryResponse);
            rateConverterService.Setup(x => x.Convert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<RateConverterData>())).Returns(2);
            httpContextAccessor.Setup(x => x.HttpContext).Returns(mockHttpContext.Object);
            mockHttpContext.Setup(x=> x.Items).Returns(items);

            var handler = new ExchangeTradeCommandHandler(mediator.Object,
                exchangeCurrencyConfiguration,
                httpContextAccessor.Object,
                logger.Object,
                rateConverterService.Object);

            //Act
            var response = await handler.Handle(request, CancellationToken.None);
            //Assert
            Assert.True(response.Success);
            Assert.Equal(request.Amount * 2, response.ConvertedAmount);
        }
    }
}
