using System;

namespace TradingSystemWebApiContract
{
    [Serializable]
    public class OrderUpdateDto
    {
        public string Id { get; set; }
        public decimal NewPrice { get; set; }
        public int NewQuantity{ get; set; }
    }
}