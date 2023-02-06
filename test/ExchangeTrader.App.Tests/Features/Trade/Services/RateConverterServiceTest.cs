using ExchangeTrader.App.Abstractions.Exchange.Dtos;
using ExchangeTrader.App.Features.Trade.Services;

namespace ExchangeTrader.App.Tests.Features.Trade.Services
{
    public class RateConverterServiceTest
    {
        [Fact]
        public void Convert_Should_Be_Converted_Successfully_When_Base_Currency_Equals_To_System_Currency()
        {
            //Arrange
            var baseCurrency = "EUR";
            var targetCurrency = "TRY";
            var rateConvertedData = new RateConverterData
            {
                SystemCurrency = baseCurrency,
                Rates = new List<Abstractions.Exchange.Dtos.Rate>
                {
                    new Abstractions.Exchange.Dtos.Rate { Code = "USD", Value = 1.08 },
                    new Abstractions.Exchange.Dtos.Rate { Code = "TRY", Value = 20.38 }
                }
            };
            //Act
            var service = new RateConverterService();
            var result = service.Convert(baseCurrency, targetCurrency, rateConvertedData);
            //Assert
            Assert.Equal(20.38, result);
        }
        [Fact]
        public void Convert_Should_Be_Converted_Successfully_When_Target_Currency_Equals_To_System_Currency()
        {
            //Arrange
            var baseCurrency = "TRY";
            var targetCurrency = "EUR";
            var rateConvertedData = new RateConverterData
            {
                SystemCurrency = targetCurrency,
                Rates = new List<Abstractions.Exchange.Dtos.Rate>
                {
                    new Abstractions.Exchange.Dtos.Rate { Code = "USD", Value = 1.08 },
                    new Abstractions.Exchange.Dtos.Rate { Code = "TRY", Value = 20.00 }
                }
            };
            //Act
            var service = new RateConverterService();
            var result = service.Convert(baseCurrency, targetCurrency, rateConvertedData);
            //Assert
            Assert.Equal(0.05, result);
        }
        [Fact]
        public void Convert_Should_Be_Converted_Successfully_When_System_Currency_Is_Not_Equal_To_Base_Currency_And_Target_Currency()
        {
            //Arrange
            var baseCurrency = "USD";
            var targetCurrency = "TRY";
            var rateConvertedData = new RateConverterData
            {
                SystemCurrency = "EUR",
                Rates = new List<Abstractions.Exchange.Dtos.Rate>
                {
                    new Abstractions.Exchange.Dtos.Rate { Code = "USD", Value = 1.00 },
                    new Abstractions.Exchange.Dtos.Rate { Code = "TRY", Value = 20.00 }
                }
            };
            //Act
            var service = new RateConverterService();
            var result = service.Convert(baseCurrency, targetCurrency, rateConvertedData);
            //Assert
            Assert.Equal(20, result);
        }
    }
}
