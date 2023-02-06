using ExchangeTrader.App.Abstractions.Auth.Configuratios;
using ExchangeTrader.App.Auth;

namespace ExchangeTrader.App.Tests.Auth
{
    public class ConfigAuthenticationServiceTest
    {
        private readonly ApiKeyConfiguration _config;
        public ConfigAuthenticationServiceTest() 
        {
            _config = new ApiKeyConfiguration()
            {
                ValidApiKeys = new string[]
                {
                    "abc", "cde"
                }
            };
        }
        [Fact]
        public async void IsApiKeyAvailable_Should_Return_Success_When_Key_Is_Valid()
        {
            //Arrange
            var apiKey = "abc";
            //Act
            var service = new ConfigAuthenticationService(_config);
            var result = await service.IsApiKeyAvailable(apiKey);
            //Assert
            Assert.True(result);
        }
        [Fact]
        public async void IsApiKeyAvailable_Should_Return_False_When_Key_Is_Not_Valid()
        {
            //Arrange
            var apiKey = "asdf";
            //Act
            var service = new ConfigAuthenticationService(_config);
            var result = await service.IsApiKeyAvailable(apiKey);
            //Assert
            Assert.False(result);
        }
    }
}
