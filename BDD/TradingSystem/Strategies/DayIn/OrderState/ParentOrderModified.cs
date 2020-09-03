using TradingSystem.Strategies.DayIn.AuctionStates;

namespace TradingSystem.Strategies.DayIn.OrderState
{
    public class ParentOrderModified : ITsOrderState
    {
        private readonly TsOrder _parentOrder;
        private readonly IAuctionState _auctionState;

        public ParentOrderModified(TsOrder parentOrder, IAuctionState auctionState)
        {
            _parentOrder = parentOrder;
            _auctionState = auctionState;
        }
        public IAuctionState HandleOrderChange()
        {
            _auctionState.OnParentModificationRequested(_parentOrder);
            return _auctionState;
        }
    }
}