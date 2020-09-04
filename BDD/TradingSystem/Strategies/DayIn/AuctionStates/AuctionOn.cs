namespace TradingSystem.Strategies.DayIn.AuctionStates
{
    public class AuctionOn : IAuctionState
    {
        public AuctionOn(StrategyContext ctx)
        {
            Context = ctx;
        }

        public IAuctionState Start()
        {
            var child = Context.OrderManager.SendChild(Context.ParentOrder);
            Context = Context.WithNewChild(child);
            return this;
        }

        public StrategyContext Context { get; private set; }

        public IAuctionState OnTradingSessionChanged(TradingSessionType newSessionType)
        {
            if (TradingSessionTypeHelper.IsAuctionOff(newSessionType))
            {
                Context.OrderManager.CancelChild(Context.ChildOrder);
            }

            return AuctionStateFactory.From(Context.WithNew(newSessionType));
        }

        public void OnParentModificationRequested(TsOrder newParent)
        {
            Context.OrderManager.Reject(newParent);
        }

        public IAuctionState OnParentCancellationRequested()
        {
            Context.OrderManager.CancelChild(Context.ChildOrder);
            Context.OrderManager.Acknowledge(Context.ParentOrder);
            return AuctionStateFactory.Finished(Context);
        }

        public void OnChildRejected()
        {
            Context.OrderManager.SendChild(Context.ParentOrder);
        }

        public IAuctionState OnChildFilled(TsOrder child)
        {
            var refreshedParent = Context.OrderManager.Refresh(Context.ParentOrder);
            var newContext = Context.WithNew(refreshedParent, child);

            if (child.PendingQuantity > 0)
            {
                Context = newContext;
                return this;
            }
            else
            {
                return AuctionStateFactory.Finished(newContext);
            }
        }
    }
}