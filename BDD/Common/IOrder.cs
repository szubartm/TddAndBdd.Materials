using System;

namespace Common
{
    public interface IOrder
    {
        string Id { get; }
        string Symbol { get; }
        Side Side { get; }
        decimal Price { get; }
        int Quantity { get; }
        TimeInForce TimeInForce { get; }
        State State { get; }
        State? CancelState { get; }
        string ParentOrderId { get; }
        DateTime Timestamp { get; }
        StrategyTypes Strategy { get; }
        int PendingQuantity { get; }
        int FilledQuantity { get; }

    }
}