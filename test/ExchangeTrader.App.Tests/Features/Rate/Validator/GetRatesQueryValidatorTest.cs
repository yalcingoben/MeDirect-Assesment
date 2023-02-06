using ExchangeTrader.App.Features.Rate.Query;
using ExchangeTrader.App.Features.Rate.Validator;

namespace ExchangeTrader.App.Tests.Features.Rate.Validator
{
    public class GetRatesQueryValidatorTest
    {
        [Fact]
        public void Validation_Should_Be_Fail_When_BaseCurrency_Is_Empty()
        {
            //Arrange
            var request = new GetRatesQuery();
            var validator = new GetRatesQueryValidator();

            //Act
            var result = validator.Validate(request);
            var exists = result.Errors.Any(p => p.PropertyName == "BaseCurrency" && p.ErrorCode == "NotEmptyValidator");

            //Assert
            Assert.False(result.IsValid);
            Assert.True(exists);
        }

        [Fact]
        public void Validation_Should_Be_Fail_When_Lenght_Of_BaseCurrency_Is_Not_Equal_Three()
        {
            //Arrange
            var request = new GetRatesQuery() { BaseCurrency = "TRYL" };
            var validator = new GetRatesQueryValidator();

            //Act
            var result = validator.Validate(request);
            var exists = result.Errors.Any(p => p.PropertyName == "BaseCurrency" && p.ErrorCode == "ExactLengthValidator");

            //Assert
            Assert.False(result.IsValid);
            Assert.True(exists);
        }
    }
}
