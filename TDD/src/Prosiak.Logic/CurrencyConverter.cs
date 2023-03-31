using System.Threading;

namespace Prosiak.Logic
{
    public interface ICurrencyConverter
    {
        decimal ConvertFromUsdToPln(decimal priceInUsd);
    }

    internal class CurrencyConverter : ICurrencyConverter
    {
        public decimal ConvertFromUsdToPln(decimal priceInUsd)
        {
            Thread.Sleep(5000); //simulate long-lasting operation
            return 3.80m * priceInUsd;
        }
    }
}