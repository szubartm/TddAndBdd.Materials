namespace TradingSystem.Strategies.DayIn
{
    internal static class TsOrderHelper
    {
        public static bool IsValid(TsOrder order)
        {
            if (order.Quantity > Settings.DayInAuctionMaxSize ||
                order.Quantity < Settings.DayInAuctionMinSize) return false;

            if (decimal.Compare(order.Price, Settings.DayInAuctionMaxPrize) > 0) return false;

            return true;
        }
    }
}