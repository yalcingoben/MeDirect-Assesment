using ExchangeTrader.App.Features.Rate.Validator;
using ExchangeTrader.App.Features.Trade.Models;
using ExchangeTrader.App.Features.Trade.Validator;

namespace ExchangeTrader.App.Tests.Features.Trade.Validator
{
    public class ExchangeTradeCommandValidatorTest
    {
        [Theory]
        [InlineData("Base")]
        [InlineData("Target")]
        public void Validation_Should_Be_Fail_When_Given_Fiels_Are_Empty(string field)
        {
            //Arrange
            var request = new ExchangeTradeCommand();
            var validator = new ExchangeTradeCommandValidator();

            //Act
            var result = validator.Validate(request);
            var exists = result.Errors.Any(p => p.PropertyName == field && p.ErrorCode == "NotEmptyValidator");

            //Assert
            Assert.False(result.IsValid);
            Assert.True(exists);
        }

        [Theory]
        [InlineData("EU")]
        [InlineData("EURO")]
        public void Validation_Should_Be_Fail_When_Lenght_Of_Given_Fiels_Is_Not_Equal_Three(string field)
        {
            //Arrange
            var request = new ExchangeTradeCommand()
            {
                Base = field
            };
            var validator = new ExchangeTradeCommandValidator();

            //Act
            var result = validator.Validate(request);
            var exists = result.Errors.Any(p => p.PropertyName == "Base" && p.ErrorCode == "ExactLengthValidator");

            //Assert
            Assert.False(result.IsValid);
            Assert.True(exists);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void Validation_Should_Be_Fail_When_Amount_Is_Not_Larger_Then_Zero(double field)
        {
            //Arrange
            var request = new ExchangeTradeCommand()
            {
                Amount = field
            };
            var validator = new ExchangeTradeCommandValidator();

            //Act
            var result = validator.Validate(request);
            var exists = result.Errors.Any(p => p.PropertyName == "Amount" && p.ErrorCode == "GreaterThanValidator");

            //Assert
            Assert.False(result.IsValid);
            Assert.True(exists);
        }

        [Fact]
        public void Validation_Should_Be_Fail_When_Base_Is_Equal_To_Target()
        {
            //Arrange
            var request = new ExchangeTradeCommand()
            {
                Base = "EUR",
                Target = "EUR"
            };
            var validator = new ExchangeTradeCommandValidator();

            //Act
            var result = validator.Validate(request);
            var exists = result.Errors.Any(p => p.PropertyName == "Target" && p.ErrorCode == "NotEqualValidator");

            //Assert
            Assert.False(result.IsValid);
            Assert.True(exists);
        }
    }
}
