namespace TradingSystem.Strategies.DayIn
{
    public class StrategyContext
    {
        public StrategyContext(
            TsOrder parentOrder,
            TradingSessionType sessionType,
            OrderManagerWrapper orderManager,
            StrategyFinishedProcessingHandler strategyFinishedProcessing,
            TsOrder childOrder = null)
        {
            ParentOrder = parentOrder;
            SessionType = sessionType;
            OrderManager = orderManager;
            StrategyFinishedProcessing = strategyFinishedProcessing;
            ChildOrder = childOrder;
        }

        public TsOrder ParentOrder { get; }
        public TradingSessionType SessionType { get; }
        public OrderManagerWrapper OrderManager { get; }
        public StrategyFinishedProcessingHandler StrategyFinishedProcessing { get; }
        public TsOrder ChildOrder { get; }

        public StrategyContext WithNew(TradingSessionType sessionType, TsOrder childOrder)
        {
            return new StrategyContext(ParentOrder, sessionType, OrderManager, StrategyFinishedProcessing, childOrder);
        }

        public StrategyContext WithNew(TradingSessionType sessionType)
        {
            return new StrategyContext(ParentOrder, sessionType, OrderManager, StrategyFinishedProcessing, ChildOrder);
        }

        public StrategyContext WithNew(TsOrder parentOrder)
        {
            return new StrategyContext(parentOrder, SessionType, OrderManager, StrategyFinishedProcessing, ChildOrder);
        }

        public StrategyContext WithNew(TsOrder parentOrder, TsOrder childOrder)
        {
            return new StrategyContext(parentOrder, SessionType, OrderManager, StrategyFinishedProcessing, childOrder);
        }

        public StrategyContext WithNewChild(TsOrder childOrder)
        {
            return new StrategyContext(ParentOrder, SessionType, OrderManager, StrategyFinishedProcessing, childOrder);
        }
    }
}