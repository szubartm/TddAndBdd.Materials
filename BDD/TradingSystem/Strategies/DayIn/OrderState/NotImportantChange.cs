using TradingSystem.Strategies.DayIn.AuctionStates;

namespace TradingSystem.Strategies.DayIn.OrderState
{
    public class NotImportantChange : ITsOrderState
    {
        private readonly IAuctionState _auctionState;

        public NotImportantChange(IAuctionState auctionState)
        {
            _auctionState = auctionState;
        }

        public IAuctionState HandleOrderChange()
            => _auctionState;
    }
}