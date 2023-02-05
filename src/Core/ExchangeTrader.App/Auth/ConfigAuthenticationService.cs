using ExchangeTrader.App.Abstractions.Auth;
using ExchangeTrader.App.Abstractions.Auth.Configuratios;

namespace ExchangeTrader.App.Auth
{
    public class ConfigAuthenticationService : IAuthenticationService
    {
        private readonly ApiKeyConfiguration _config;

        public ConfigAuthenticationService(ApiKeyConfiguration config)
        {
            _config = config;
        }

        public Task<bool> IsApiKeyAvailable(string apiKey)
        {
            return Task.FromResult(_config.ValidApiKeys.Contains(apiKey, StringComparer.InvariantCultureIgnoreCase));
        }
    }
}
