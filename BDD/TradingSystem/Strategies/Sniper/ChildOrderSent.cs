using Common;
using DataProviderSystem;

namespace TradingSystem.Strategies.Sniper
{
    public class ChildOrderSent : ISniperState
    {
        private readonly StrategyContext _context;
        private TsOrder _childOrder;
        public TsOrder ParentOrder { get; private set; }

        public ChildOrderSent(StrategyContext context,
            TsOrder parentOrder,
            TsOrder childOrder)
        {
            _context = context;
            ParentOrder = parentOrder;
            _childOrder = childOrder;
        }
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
            switch (changeType)
            {
                case OrderChangeType.Rejection:
                case OrderChangeType.Cancellation:
                    ParentOrder = _context.OrderManagerWrapper.Refresh(ParentOrder);
                    return new WaitingForMarketData(_context, ParentOrder);
                case OrderChangeType.Fill when childOrder.PendingQuantity == 0:
                    ParentOrder = _context.OrderManagerWrapper.Refresh(ParentOrder);
                    _context.StrategyFinishedProcessing(ParentOrder.Id);
                    return new ChildOrderFilled(ParentOrder);
                default:
                    _childOrder = childOrder;
                    return this;
            }
        }

        public ISniperState ParentOrderChanged(OrderChangeType changeType, TsOrder parentOrder)
        {
            switch (changeType)
            {
                case OrderChangeType.Cancellation:
                {
                    var cancelSucceded = _context.OrderManagerWrapper.CancelChild(_childOrder);
                    if (cancelSucceded)
                    {
                        _context.OrderManagerWrapper.Acknowledge(parentOrder);
                        ParentOrder = _context.OrderManagerWrapper.Refresh(ParentOrder);
                        _context.StrategyFinishedProcessing(ParentOrder.Id);
                        return new ParentOrderCancelled(ParentOrder);
                    }
                
                    _context.OrderManagerWrapper.Reject(parentOrder);
                    return this;
                }
                case OrderChangeType.Modification:
                {
                    var priceChangeBetter = IsPriceLimitBetter(parentOrder);
                    var anyPendingQuantityLeft = ParentOrder.PendingQuantity < parentOrder.PendingQuantity;

                    if (!anyPendingQuantityLeft || (anyPendingQuantityLeft && !priceChangeBetter))
                    {
                        _context.OrderManagerWrapper.Reject(parentOrder);
                        ParentOrder = parentOrder;
                        return this;
                    }

                    _childOrder = _context.OrderManagerWrapper.ModifyChild(_childOrder, parentOrder.Price, parentOrder.PendingQuantity);
                    break;
                }
            }

            ParentOrder = parentOrder;
            return this;
        }

        private bool IsPriceLimitBetter(IOrder newOrder)
        {
            if (ParentOrder.Side == Side.Buy) return ParentOrder.Price <= newOrder.Price;
            return ParentOrder.Price >= newOrder.Price;
        }
    }
}