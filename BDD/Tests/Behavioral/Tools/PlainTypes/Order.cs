using Common;

namespace Behavioral.Tools.PlainTypes
{
    public class Order
    {
        public string Id { get; set; }
        public string Symbol { get; set;}= "AZN.L";
        public Side Side { get; set;}= Side.Buy;
        public decimal Price { get; set;}= 100;
        public int Quantity { get; set;}= 10000;
        public TimeInForce TimeInForce { get; set;}= TimeInForce.Day;
        public string ParentOrderId { get; set;}
        public StrategyTypes StrategyType { get; set;}= StrategyTypes.Day;

    }
}