using System;
using Common;
using DataProviderSystem;
using OrderManagementSystem;
using TradingSystem.Strategies.DayIn.AuctionStates;
using TradingSystem.Strategies.DayIn.OrderState;

namespace TradingSystem.Strategies.DayIn
{
    public class DayInAuction : IStrategy
    {
        public event StrategyFinishedProcessingHandler StrategyFinishedProcessing;

        private IAuctionState _auctionState;

        public DayInAuction(
            TsOrder parentOrder, 
            TradingSessionType sessionType,
            IOrderManager orderManager)
        {
            _auctionState = AuctionStateFactory.Created(
                new StrategyContext(parentOrder, sessionType, new OrderManagerWrapper(orderManager), StrategyFinishedProcessing));
        }

        public void Start()
        {
            _auctionState = _auctionState.Start();
        }

        public void onNewMarketData(MarketData marketData)
        {
        }

        public void onTradingSessionChanged(TradingSessionType session)
        {
            _auctionState = _auctionState.OnTradingSessionChanged(session);
        }

        public void onOrderChanged(OrderChangeType changeType, IOrder omsOrder)
        {
            var order = TsOrder.From(omsOrder);
            _auctionState = OrderStateFactory
                .Create(changeType, order, _auctionState)
                .HandleOrderChange();
        }

        public void onTimeChanged(DateTime time)
        {
        }

        public TsOrder ParentOrder => _auctionState.Context.ParentOrder;
    }
}