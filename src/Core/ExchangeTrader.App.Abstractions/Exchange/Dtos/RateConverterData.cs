namespace ExchangeTrader.App.Abstractions.Exchange.Dtos
{
    public class RateConverterData
    {
        public string BaseCurrency { get; set; }
        public IEnumerable<Rate> Rates { get; set; }
    }
}
