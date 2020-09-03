using System.Threading.Tasks;

namespace Behavioral.Tools
{
    public interface IMarketDataServiceSimulator
    {
        Task InsertNewMarketData(string symbol, decimal bid, decimal ask, int bidSize, int askSize);
        Task ClearMarketData();
    }
}