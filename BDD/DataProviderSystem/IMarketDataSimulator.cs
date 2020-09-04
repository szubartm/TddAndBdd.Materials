namespace DataProviderSystem
{
    public interface IMarketDataSimulator
    {
        void InsertNewMarketData(string symbol, decimal bid, decimal ask, int bidSize, int askSize);
        void ClearMarketData();
    }
}