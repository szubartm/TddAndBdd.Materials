namespace Prosiak.Logic;

public interface IPigFoodPriceProvider
{
    decimal GetDailyPriceOfFoodFor(Pig pig);
}

internal record PigFoodPriceProvider(IPotatoPriceProvider PotatoPriceProvider,
    ICurrencyConverter CurrencyConverter) : IPigFoodPriceProvider
{
    public decimal GetDailyPriceOfFoodFor(Pig pig)
    {
        var pricePerKgInUsd = PotatoPriceProvider.GetPriceOfHundredKilogramPotatoesFromStockExchange() / 100;
        var pricePerKgInPln = CurrencyConverter.ConvertFromUsdToPln(pricePerKgInUsd);

        return pig.Age.TotalDays switch
        {
            < 100 => 2 * pricePerKgInPln,
            < 200 => 3.5m * pricePerKgInPln,
            _ => 3.25m * pricePerKgInPln,
        };
    }
}
