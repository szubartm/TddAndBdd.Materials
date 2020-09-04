using TradingSystem.Strategies.DayIn.AuctionStates;

namespace TradingSystem.Strategies.DayIn.OrderState
{
    public class ChildOrderModified : ITsOrderState
    {
        private readonly TsOrder _parentOrder;
        private readonly IAuctionState _auctionState;

        public ChildOrderModified(TsOrder parentOrder, IAuctionState auctionState)
        {
            _parentOrder = parentOrder;
            _auctionState = auctionState;
        }

        public IAuctionState HandleOrderChange()
        {
            return _auctionState.OnChildFilled(_parentOrder);
        }
    }
}