using DataProviderSystem;
using OrderManagementSystem;
using TradingSystem;
using TradingSystem.Reporting;
using TradingSystem.Strategies;
using TradingSystem.Validation;

namespace TradingSystemConsole
{
    public class TradingEnvironment
    {
        public TradingEnvironment()
        {
            OrderManager = new OrderManager();
            MarketDataSimulator = new MarketDataSimulator();
            TimeSimulator = new TimeSimulator();
            TradingSystem = new TradingSystemService(1, OrderManager, MarketDataSimulator,
                new SimpleStrategyFactory(), new ConsoleReporter(), TimeSimulator, new SettingsValidator());
        }

        public TradingEnvironment(TradingSystemService tradingSystem, IOrderManager orderManager,
            MarketDataSimulator marketDataSimulator, ITimeSimulator timeSimulator)
        {
            TradingSystem = tradingSystem;
            OrderManager = orderManager;
            MarketDataSimulator = marketDataSimulator;
            TimeSimulator = timeSimulator;
        }

        public TradingSystemService TradingSystem { get; }
        public IOrderManager OrderManager { get; }
        public MarketDataSimulator MarketDataSimulator { get; }
        public ITimeSimulator TimeSimulator { get; }
    }
}