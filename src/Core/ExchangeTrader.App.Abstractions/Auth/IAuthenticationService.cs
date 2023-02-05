namespace ExchangeTrader.App.Abstractions.Auth
{
    public interface IAuthenticationService
    {
        public Task<bool> IsApiKeyAvailable(string apiKey);
    }
}
