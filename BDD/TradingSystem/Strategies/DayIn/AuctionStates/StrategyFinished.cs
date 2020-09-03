namespace TradingSystem.Strategies.DayIn.AuctionStates
{
    public class StrategyFinished : IAuctionState
    {
        public StrategyFinished(StrategyContext context)
        {
            Context = context;
        }

        public IAuctionState Start()
        {
            return this;
        }

        public StrategyContext Context { get; }
        public IAuctionState OnTradingSessionChanged(TradingSessionType newSessionType)
        {
            return this;
        }

        public void OnParentModificationRequested(TsOrder newParent)
        {
        }

        public IAuctionState OnParentCancellationRequested()
        {
            return this;
        }

        public void OnChildRejected()
        {
        }

        public IAuctionState OnChildFilled(TsOrder child)
        {
            return this;
        }
    }
}