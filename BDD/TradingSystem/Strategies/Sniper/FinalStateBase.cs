using Common;
using DataProviderSystem;

namespace TradingSystem.Strategies.Sniper
{
    public abstract class FinalStateBase :ISniperState
    {
        protected FinalStateBase(TsOrder parentOrder)
        {
            ParentOrder = parentOrder;
        }

        public TsOrder ParentOrder { get; }

        public ISniperState Validate()
        {
            return this;
        }

        public ISniperState MarketDataReceived(MarketData marketData)
        {
            return this;
        }

        public ISniperState ChildOrderChanged(OrderChangeType changeType, TsOrder childOrder)
        {
            return this;
        }

        public ISniperState ParentOrderChanged(OrderChangeType changeType, TsOrder parentOrder)
        {
            return this;
        }
    }
}