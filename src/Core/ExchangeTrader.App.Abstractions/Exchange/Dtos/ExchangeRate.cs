namespace ExchangeTrader.App.Abstractions.Exchange.Dtos
{
    public class ExchangeRate
    {
        public List<Rate> Rates { get; set; }
        public DateTime Date { get; set; }
    }
}
