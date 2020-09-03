using System;
using Common;

namespace TradingSystemWebApiContract
{
    [Serializable]
    public class OrderDto
    {
        public string Symbol { get; set; }
        public Side Side{ get; set; }
        public decimal Price{ get; set; }
        public int Quantity{ get; set; }
        public TimeInForce TimeInForce{ get; set; }
        public string ParentOrderId{ get; set; }
        public StrategyTypes Strategy{ get; set; }
    }
}
