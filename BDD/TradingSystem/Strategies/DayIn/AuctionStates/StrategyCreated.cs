namespace TradingSystem.Strategies.DayIn.AuctionStates
{
    public class StrategyCreated : IAuctionState
    {
        public StrategyCreated(StrategyContext context)
        {
            Context = context;
        }
        public IAuctionState Start()
        {
            if (!TsOrderHelper.IsValid(Context.ParentOrder))
            {
                Context.OrderManager.Reject(Context.ParentOrder);
                return AuctionStateFactory.Finished(Context);
            }
            else
            {
                Context.OrderManager.Acknowledge(Context.ParentOrder);
                return AuctionStateFactory.From(Context).Start();
            }
        }

        public StrategyContext Context { get; }

        public IAuctionState OnTradingSessionChanged(TradingSessionType newSessionType)
            => this;

        public void OnParentModificationRequested(TsOrder newParent) {}

        public IAuctionState OnParentCancellationRequested()
            => this;

        public void OnChildRejected() {}

        public IAuctionState OnChildFilled(TsOrder child)
        {
            return this;
        }
    }
}