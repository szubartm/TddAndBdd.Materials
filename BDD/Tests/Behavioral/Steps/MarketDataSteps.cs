using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Behavioral.Steps
{
    [Binding]
    public class MarketDataSteps
    {
        [When(@"new market data is received (.*): (\d+)@(\d+) - (\d+)@(\d+)")]
        public async Task NewMarketDataIsReceived(string symbol, int bidSize, decimal bid, int askSize, decimal ask)
        {
            await Hooks.Hooks.SimulationContext.MarketDataServiceSimulator.InsertNewMarketData(symbol, bid, ask,
                bidSize, askSize);
        }
    }
}