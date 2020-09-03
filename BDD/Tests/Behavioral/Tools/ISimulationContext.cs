namespace Behavioral.Tools
{
    public interface ISimulationContext
    {
        IMarketDataServiceSimulator MarketDataServiceSimulator { get; }
        IOrderManagementSystemSimulator OrderManagementSystemSimulator { get; }
        ITradingSystemClient TradingSystemClient { get; }
    }
}