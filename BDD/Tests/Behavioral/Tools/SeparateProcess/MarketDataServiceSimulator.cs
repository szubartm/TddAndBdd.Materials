using System;
using System.Net.Http;
using System.Threading.Tasks;
using TradingSystemWebApiContract;

namespace Behavioral.Tools.SeparateProcess
{
    public class MarketDataServiceSimulator : IMarketDataServiceSimulator
    {
        private readonly HttpClient _client;

        public MarketDataServiceSimulator(HttpClient client)
        {
            _client = client;
        }

        public async Task InsertNewMarketData(string symbol, decimal bid, decimal ask, int bidSize, int askSize)
        {
            var result = await _client.PostAsync(@"marketData/insert", Tools.PrepareStringContent(new MarketDataDto(){Ask = ask, AskSize = askSize, BidSize = bidSize, Bid = bid, Symbol = symbol}));
            result.EnsureSuccessStatusCode();
        }

        public async Task ClearMarketData()
        {
            var result = await _client.PostAsync(@"marketData/clear", new StringContent(string.Empty));
            result.EnsureSuccessStatusCode();
        }
    }
}