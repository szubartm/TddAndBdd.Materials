using Common;
using DataProviderSystem;

namespace TradingSystem.Strategies.Sniper
{
    public class StrategyCreated :ISniperState
    {
        public TsOrder ParentOrder { get; }
        private readonly StrategyContext _context;
       
        public StrategyCreated(
            StrategyContext context,
            TsOrder parentOrder)
        {
            ParentOrder = parentOrder;
            _context = context;
        }
        public ISniperState Validate()
        {
            var isInvalid = ParentOrder.Quantity > Settings.SniperMaxSize ||
                          ParentOrder.Quantity < Settings.SniperMinSize ||
                          decimal.Compare(ParentOrder.Price, Settings.SniperMaxPrize) > 0 ||
                          decimal.Compare(ParentOrder.Price, Settings.SniperMinPrize) < 0;

            if (!isInvalid)
            {
                _context.OrderManagerWrapper.Acknowledge(ParentOrder);
                return new WaitingForMarketData(_context, ParentOrder);
            }
            
            _context.OrderManagerWrapper.Reject(ParentOrder);
            _context.StrategyFinishedProcessing(ParentOrder.Id);
            return new ParentOrderInvalid(ParentOrder);
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