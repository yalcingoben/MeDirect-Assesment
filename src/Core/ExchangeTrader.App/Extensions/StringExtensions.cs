namespace ExchangeTrader.App.Extensions
{
    public static class StringExtensions
    {
        public static bool EqualsInvariantCulture(this string value, string target)
            => value.Equals(target, StringComparison.InvariantCultureIgnoreCase);
    }
}
