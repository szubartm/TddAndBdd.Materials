namespace DataProviderSystem
{
    public class MarketData
    {
        public MarketData(string symbol, decimal bid, decimal ask, int bidSize, int askSize)
        {
            Symbol = symbol;
            Bid = bid;
            Ask = ask;
            BidSize = bidSize;
            AskSize = askSize;
        }

        public string Symbol { get; }
        public decimal Bid { get; }
        public decimal Ask { get; }
        public int BidSize { get; }
        public int AskSize { get; }
    }
}