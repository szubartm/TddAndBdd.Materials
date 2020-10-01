using System.Threading;

namespace Prosiak.Logic
{
    public interface IPotatoPriceProvider
    {
        decimal GetPriceOfHundredKilogramPotatoesFromStockExchange();
    }

    internal class PotatoPriceProvider : IPotatoPriceProvider
    {
        public decimal GetPriceOfHundredKilogramPotatoesFromStockExchange()
        {
            Thread.Sleep(5000); //let's assume we need to get to the exchange via slow web service
            return 135m;
        }
    }
}