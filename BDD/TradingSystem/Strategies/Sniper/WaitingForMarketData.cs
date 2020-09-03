using Common;
using DataProviderSystem;

namespace TradingSystem.Strategies.Sniper
{
    public class WaitingForMarketData : ISniperState
    {
        public TsOrder ParentOrder { get; private set; }
        private readonly StrategyContext _context;


        public WaitingForMarketData(
            StrategyContext context,
            TsOrder parentOrder)
        {
            ParentOrder = parentOrder;
            _context = context;
        }

        public ISniperState Validate()
        {
            return this;
        }

        public ISniperState MarketDataReceived(MarketData marketData)
        {
            var isMarketDataMatchingCriteria = ((ParentOrder.Side == Side.Buy && marketData.Ask <= ParentOrder.Price) ||
                                              (ParentOrder.Side == Side.Sell && marketData.Bid >= ParentOrder.Price));

            if (isMarketDataMatchingCriteria)
            {
                var childOrder = _context.OrderManagerWrapper.SendChild(ParentOrder, TimeInForce.FOK);
                return new ChildOrderSent(_context, ParentOrder, childOrder);
            }

            return this;
        }

        public ISniperState ChildOrderChanged(OrderChangeType changeType, TsOrder childOrder)
        {
            return this;
        }

        public ISniperState ParentOrderChanged(OrderChangeType changeType, TsOrder parentOrder)
        {
            if (changeType == OrderChangeType.Cancellation)
            {
                _context.OrderManagerWrapper.Acknowledge(parentOrder);
                _context.StrategyFinishedProcessing(ParentOrder.Id);
                return new ParentOrderCancelled(parentOrder);
            }

            if (changeType == OrderChangeType.Modification)
            {
                return new StrategyCreated(_context, parentOrder);
            }

            ParentOrder = parentOrder;
            return this;
        }
    }
}