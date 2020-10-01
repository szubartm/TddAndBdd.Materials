namespace Prosiak.Logic
{
    public interface IPigFoodPriceProvider
    {
        decimal GetDailyPriceOfFoodFor(Pig pig);
    }

    internal class PigFoodPriceProvider : IPigFoodPriceProvider
    {
        private readonly ICurrencyConverter _currencyConverter;
        private readonly IPotatoPriceProvider _potatoPriceProvider;

        public PigFoodPriceProvider(IPotatoPriceProvider potatoPriceProvider, ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
            _potatoPriceProvider = potatoPriceProvider;
        }

        public decimal GetDailyPriceOfFoodFor(Pig pig)
        {
            var pricePerKgInUsd = _potatoPriceProvider.GetPriceOfHundredKilogramPotatoesFromStockExchange() / 100;
            var pricePerKgInPln = _currencyConverter.ConvertFromUsdToPln(pricePerKgInUsd);

            if (pig.Age.TotalDays < 100)
                return 2 * pricePerKgInPln;
            else if (pig.Age.TotalDays < 200)
                return 3.5m * pricePerKgInPln;
            else return 3.25m * pricePerKgInPln;
        }
    }
}