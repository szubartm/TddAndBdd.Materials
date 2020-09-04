namespace TradingSystem.Strategies.DayIn.AuctionStates
{
    public interface IAuctionState
    {
        IAuctionState Start();
        StrategyContext Context { get; }
        IAuctionState OnTradingSessionChanged(TradingSessionType newSessionType);

        void OnParentModificationRequested(TsOrder newParent);
        IAuctionState OnParentCancellationRequested();

        void OnChildRejected();

        IAuctionState OnChildFilled(TsOrder child);
    }
}