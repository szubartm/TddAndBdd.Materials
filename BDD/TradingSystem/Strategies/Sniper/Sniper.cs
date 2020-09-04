using System;
using Common;
using DataProviderSystem;
using OrderManagementSystem;

namespace TradingSystem.Strategies.Sniper
{
    public class Sniper :IStrategy
    {
        public event StrategyFinishedProcessingHandler StrategyFinishedProcessing;
        private ISniperState _state;
        private StrategyContext _context;

        public Sniper(IOrderManager orderManager, IOrder order)
        {
            _context = new StrategyContext(new OrderManagerWrapper(orderManager), StrategyFinishedProcessing);
            _state = new StrategyCreated(_context, TsOrder.From(order));
        }
        public void Start()
        {
            _context.StrategyFinishedProcessing = StrategyFinishedProcessing;
            _state = _state.Validate();
        }

        public void onNewMarketData(MarketData marketData)
        {
            _state = _state.MarketDataReceived(marketData);
        }

        public void onTradingSessionChanged(TradingSessionType session)
        {
        }

        public void onOrderChanged(OrderChangeType changeType, IOrder omsOrder)
        {
            var order = TsOrder.From(omsOrder);
            _state = IsParentOrder(order) 
                ? _state.ParentOrderChanged(changeType, order) 
                : _state.ChildOrderChanged(changeType, order);
        }

        private static bool IsParentOrder(TsOrder order)
        {
            return order.ParentOrderId == null;
        }

        public void onTimeChanged(DateTime time)
        {
        }

        public TsOrder ParentOrder => _state.ParentOrder;
    }
}