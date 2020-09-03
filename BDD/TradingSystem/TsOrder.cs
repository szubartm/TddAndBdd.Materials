using System;
using Common;

namespace TradingSystem
{
    public class TsOrder : IOrder
    {
        private TsOrder(
            string id,
            string symbol,
            Side side,
            decimal price,
            int quantity,
            TimeInForce timeInForce,
            State state,
            State? cancelState,
            string parentOrderId,
            DateTime timestamp,
            StrategyTypes strategy,
            int filledQuantity)
        {
            Id = id;
            Symbol = symbol;
            Side = side;
            Price = price;
            Quantity = quantity;
            TimeInForce = timeInForce;
            State = state;
            CancelState = cancelState;
            ParentOrderId = parentOrderId;
            Timestamp = timestamp;
            Strategy = strategy;
            FilledQuantity = filledQuantity;
        }

        public string Id { get; }
        public string Symbol { get; }
        public Side Side { get; }
        public decimal Price { get; }
        public int Quantity { get; }
        public TimeInForce TimeInForce { get; }
        public State State { get; }
        public State? CancelState { get; }
        public string ParentOrderId { get; }
        public DateTime Timestamp { get; }
        public StrategyTypes Strategy { get; }
        public int PendingQuantity => Quantity - FilledQuantity;
        public int FilledQuantity { get; }

        public static TsOrder From(IOrder order)
        {
            return new TsOrder(order.Id, order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.State, order.CancelState, order.ParentOrderId, order.Timestamp, order.Strategy,
                order.FilledQuantity);
        }

        public TsOrder WithNew(decimal price, int quantity, State state)
        {
            return new TsOrder(Id, Symbol, Side, price, quantity, TimeInForce, state, CancelState, ParentOrderId,
                Timestamp, Strategy, FilledQuantity);
        }

        public TsOrder WithNew(State state)
        {
            return new TsOrder(Id, Symbol, Side, Price, Quantity, TimeInForce, state, CancelState, ParentOrderId,
                Timestamp, Strategy, FilledQuantity);
        }

        public TsOrder WithNew(State? cancelState)
        {
            return new TsOrder(Id, Symbol, Side, Price, Quantity, TimeInForce, State, cancelState, ParentOrderId,
                Timestamp, Strategy, FilledQuantity);
        }

        public TsOrder WithNew(int pendingQuantity, State state)
        {
            return new TsOrder(Id, Symbol, Side, Price, Quantity, TimeInForce, state, CancelState, ParentOrderId,
                Timestamp, Strategy, pendingQuantity);
        }
    }
}