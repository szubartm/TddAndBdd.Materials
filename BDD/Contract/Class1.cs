using System;

namespace Contract
{
    public class OrderDto
    {
        string symbol,
            Side side,
        decimal price,
        int quantity,
            TimeInForce timeInForce,
        string parentOrderId,
            StrategyTypes strategy
    }
}
