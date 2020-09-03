using System;

namespace TradingSystemWebApiContract
{
    [Serializable]
    public class OrderFillDto
    {
        public string Id { get; set; }
        public int Quantity { get; set; }
    }
}