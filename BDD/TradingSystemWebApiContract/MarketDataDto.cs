using System;

namespace TradingSystemWebApiContract
{
    [Serializable]
    public class MarketDataDto
    {
        public string Symbol { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
        public int BidSize { get; set; }
        public int AskSize { get; set; }
    }
}