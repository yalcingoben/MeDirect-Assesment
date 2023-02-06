using ExchangeTrader.App.Extensions;

namespace ExchangeTrader.App.Tests.Extensions
{
    public class StringExtensionsTest
    {
        [Fact]
        public void EqualsInvariantCulture_Should_Equal_When_Parameter_Compared()
        {
            //Arrange
            string a = "i";
            string b = "I";
            //Act
            var result = StringExtensions.EqualsInvariantCulture(a, b);
            //Assert
            Assert.True(result);
        }

        [Fact]
        public void EqualsInvariantCulture_Should_Not_Equal_When_Parameter_Compared()
        {
            //Arrange
            string a = "ı";
            string b = "I";
            //Act
            var result = StringExtensions.EqualsInvariantCulture(a, b);
            //Assert
            Assert.False(result);
        }
    }
}
