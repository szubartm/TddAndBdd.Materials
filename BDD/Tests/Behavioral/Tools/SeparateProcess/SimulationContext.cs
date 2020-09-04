using System;
using System.Net.Http;


namespace Behavioral.Tools.SeparateProcess
{
    public class SimulationContext : ISimulationContext
    {
        static HttpClient client = new HttpClient();
        public SimulationContext(string uriString)
        {
            client.BaseAddress = new Uri(uriString);//"http://localhost:5000");

            MarketDataServiceSimulator = new MarketDataServiceSimulator(client);
            OrderManagementSystemSimulator = new OrderManagementSystemSimulator(client);
            TradingSystemClient = new TradingSystemClient(client);
        }

        public IMarketDataServiceSimulator MarketDataServiceSimulator { get; }
        public IOrderManagementSystemSimulator OrderManagementSystemSimulator { get; }
        public ITradingSystemClient TradingSystemClient { get; }
    }
}