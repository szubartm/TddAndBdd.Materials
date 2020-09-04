namespace DataProviderSystem
{
    public delegate void NewMarketDataHandler(MarketData marketData);

    public interface IMarketDataService
    {
        event NewMarketDataHandler NewMarketDataReceived;
    }
}