namespace ExchangeTrader.Integration.Fixer.Models
{
    internal class SymbolsResponse
    {
        public bool Success { get; set; }
        public Dictionary<string, string> Symbols { get; set; }
    }
}
