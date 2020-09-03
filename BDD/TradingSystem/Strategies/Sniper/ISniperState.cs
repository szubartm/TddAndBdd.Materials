using Common;
using DataProviderSystem;

namespace TradingSystem.Strategies.Sniper
{
    public interface ISniperState
    {
        TsOrder ParentOrder { get; }
        ISniperState Validate();
        ISniperState MarketDataReceived(MarketData marketData);
        ISniperState ChildOrderChanged(OrderChangeType changeType, TsOrder childOrder);
        ISniperState ParentOrderChanged(OrderChangeType changeType, TsOrder parentOrder);
    }
}