using System;
using Common;
using DataProviderSystem;

namespace TradingSystem.Strategies
{
    public delegate void StrategyFinishedProcessingHandler(string orderId);

    public interface IStrategy : IStrategyEvent
    {
        TsOrder ParentOrder { get; }
    }

    public interface IStrategyEvent
    {
        event StrategyFinishedProcessingHandler StrategyFinishedProcessing;
        void Start();
        void onNewMarketData(MarketData marketData);
        void onTradingSessionChanged(TradingSessionType session);
        void onOrderChanged(OrderChangeType changeType, IOrder order);
        void onTimeChanged(DateTime time);
    }
}