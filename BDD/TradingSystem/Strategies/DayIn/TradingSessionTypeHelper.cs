namespace TradingSystem.Strategies.DayIn
{
    public class TradingSessionTypeHelper
    {
        public static bool IsAuctionOn(TradingSessionType sessionType)
            => sessionType == TradingSessionType.OpenAuction
              // || sessionType == TradingSessionType.CloseAuction
               || sessionType == TradingSessionType.IntradayAuction;

        public static bool IsAuctionOff(TradingSessionType sessionType)
            => !IsAuctionOn(sessionType);
    }
}