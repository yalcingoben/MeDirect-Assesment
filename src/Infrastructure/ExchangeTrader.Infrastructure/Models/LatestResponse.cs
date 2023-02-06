namespace ExchangeTrader.Integration.Fixer.Models
{
    public class LatestResponse
    {
        public bool Success { get; set; }
        public string Base { get; set; }
        public DateTime Date { get; set; }
        public Dictionary<string, double> Rates { get; set; }

    }
}
