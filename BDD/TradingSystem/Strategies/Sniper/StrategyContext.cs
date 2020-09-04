namespace TradingSystem.Strategies.Sniper
{
    public class StrategyContext
    {
        public OrderManagerWrapper OrderManagerWrapper { get; }
        public StrategyFinishedProcessingHandler StrategyFinishedProcessing { get; set; }


        public StrategyContext(
            OrderManagerWrapper orderManagerWrapper,
            StrategyFinishedProcessingHandler strategyFinishedProcessing)
        {
            OrderManagerWrapper = orderManagerWrapper;
            StrategyFinishedProcessing += strategyFinishedProcessing;
        }
    }
}