namespace TradingSystem.Strategies.DayIn.AuctionStates
{
    internal static class AuctionStateFactory
    {
        public static IAuctionState From(StrategyContext context)
        {
            if (TradingSessionTypeHelper.IsAuctionOn(context.SessionType))
                return new AuctionOn(context);
            return new AuctionOff(context);
        }

        public static IAuctionState Created(StrategyContext context)
        {
            return new StrategyCreated(context);
        }

        public static IAuctionState Finished(StrategyContext context)
        {
            context.StrategyFinishedProcessing?.Invoke(context.ParentOrder.Id);
            return new StrategyFinished(context);
        }
    }
}