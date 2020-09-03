using System.Collections.Generic;
using System.Linq;

namespace DataProviderSystem
{
    public class MarketDataSimulator : IMarketDataService, IMarketDataSimulator
    {
        private readonly Dictionary<string, List<MarketData>> _marketData = new Dictionary<string, List<MarketData>>();

        public event NewMarketDataHandler NewMarketDataReceived;

        public void InsertNewMarketData(string symbol, decimal bid, decimal ask, int bidSize, int askSize)
        {
            if (!_marketData.Keys.Contains(symbol)) _marketData.Add(symbol, new List<MarketData>());
            var marketData = new MarketData(symbol, bid, ask, bidSize, askSize);

            _marketData[symbol].Add(marketData);

            NewMarketDataReceived?.Invoke(marketData);
        }

        public void ClearMarketData()
        {
            _marketData.Clear();
        }
    }
}