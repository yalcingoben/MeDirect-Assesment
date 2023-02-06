using ExchangeTrader.Api.Controllers;
using ExchangeTrader.App.Features.Trade.Models;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeTrader.Api.Tests.Controller
{
    public class TradeControllerTest
    {
        private readonly Mock<IMediator> _mediator;

        public TradeControllerTest()
        {
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async void Trade_Should_Return_ConvertedAmount_When_RequestParameters_Are_Given()
        {
            //Arrange
            var command = new ExchangeTradeCommand
            {
                Base = "EUR",
                Target = "TRY",
                Amount = 100
            };
            var response = new ExchangeTradeResponse
            {
                ConvertedAmount = 200,
                Success = true
            };
            _mediator.Setup(x => x.Send(It.IsAny<ExchangeTradeCommand>(), CancellationToken.None)).ReturnsAsync(() => response);
            //Act
            var controller = new TradeController(_mediator.Object);
            var result = await controller.Trade(command);
            //Assert

            Assert.NotNull(result);
            Assert.Equal(200, result.ConvertedAmount);
        }
    }
}
