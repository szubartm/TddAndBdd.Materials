using TradingSystem.Strategies.DayIn.AuctionStates;

namespace TradingSystem.Strategies.DayIn.OrderState
{
    public interface ITsOrderState
    {
        IAuctionState HandleOrderChange();
    }
}