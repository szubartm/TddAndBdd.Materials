using TradingSystem.Strategies.DayIn.AuctionStates;

namespace TradingSystem.Strategies.DayIn.OrderState
{
    public class ChildOrderRejected : ITsOrderState
    {
        private readonly IAuctionState _auctionState;

        public ChildOrderRejected(IAuctionState auctionState)
        {
            _auctionState = auctionState;
        }

        public IAuctionState HandleOrderChange()
        {
            _auctionState.OnChildRejected();
            return _auctionState;
        }
    }
}