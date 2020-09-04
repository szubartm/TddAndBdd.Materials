namespace TradingSystem.Strategies.DayIn.AuctionStates
{
    public class AuctionOff : IAuctionState
    {
        internal AuctionOff(
            StrategyContext context)
        {
            Context = context;
        }

        public IAuctionState Start()
        {
            return this;
        }

        public StrategyContext Context { get; private set; }

        public IAuctionState OnTradingSessionChanged(TradingSessionType newSessionType)
        {
            StrategyContext newContext = null;
            if (TradingSessionTypeHelper.IsAuctionOn(newSessionType))
            {
                var child = Context.OrderManager.SendChild(Context.ParentOrder);
                newContext = Context.WithNew(newSessionType, child);
            }
            else
            {
                newContext = Context.WithNew(newSessionType);
            }

            return AuctionStateFactory.From(newContext);
        }


        public void OnParentModificationRequested(TsOrder newParent)
        {
            var oms = Context.OrderManager;

            if (TsOrderHelper.IsValid(newParent))
            {
                oms.Acknowledge(newParent);
                Context = Context.WithNew(newParent);
            }
            else
            {
                oms.Reject(newParent);
            }
        }

        public IAuctionState OnParentCancellationRequested()
        {
            var oms = Context.OrderManager;
            var parentOrder = Context.ParentOrder;

            oms.Acknowledge(parentOrder);
            return AuctionStateFactory.Finished(Context);
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