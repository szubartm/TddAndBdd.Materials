using System;
using System.Collections.Generic;
using Common;

namespace OrderManagementSystem
{
    public abstract class OmsOrderBase : IOrder
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public Side Side { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public TimeInForce TimeInForce { get; set; }
        public virtual State State { get; set; }
        public string ParentOrderId { get; set; }

        public DateTime Timestamp { get; set; }
        public StrategyTypes Strategy { get; set; }

        public abstract int PendingQuantity { get; }
        public virtual int FilledQuantity { get; set; }

        public State? CancelState { get; set; }

        public abstract void AddChild(IOrder child);
        public abstract IEnumerable<IOrder> GetAllChilds();

        public bool Fill(int qty)
        {
            if (State == State.UNACK || State == State.CLOSED || State == State.REJECTED || State == State.CANCELLED ||
                State == State.FILLED)
                return false;
            if(ParentOrderId == null) throw new NotSupportedException();

            if (PendingQuantity >= qty)
            {
                Timestamp = DateTime.Now;
                FilledQuantity += qty;
                //PendingQuantity = Quantity - FilledQuantity;
                State = PendingQuantity == 0 ? State.FILLED : State.PFILLED;
            }

            return true;
        }

        public void Acknowledge()
        {
            if (State == State.UNACK)
                State = State.LIVE;
            if (CancelState == State.PCANCEL)
            {
                State = State.CANCELLED;
                CancelState = State.CANCELLED;
            }

            if (State == State.PMOD)
                State = State.LIVE;
        }

        public bool Cancel()
        {
            if (State == State.CLOSED || State == State.REJECTED || State == State.CANCELLED ||
                State == State.FILLED) return false;
            CancelState = State.PCANCEL;
            Timestamp = DateTime.Now;
            return true;
        }

        public bool Reject()
        {
            if (State == State.CLOSED || State == State.REJECTED || State == State.CANCELLED ||
                State == State.FILLED) return false;
            if (CancelState == State.PCANCEL)
            {
                CancelState = null;
                return true;
            }

            State = State.REJECTED; //todo shouldn't be moving to previous state?
            Timestamp = DateTime.Now;
            return true;
        }

        public bool Update(decimal price, int quantity)
        {
            if (State == State.CLOSED || State == State.REJECTED || State == State.CANCELLED ||
                State == State.FILLED) return false;
            if (quantity < Quantity - PendingQuantity)
                return false;

            Price = price;
            Quantity = quantity;
            return true;
        }

        public override string ToString()
        {
            return $"Id: {Id}, Symbol: {Symbol}, Side: {Side}, Price: {Price}, Quantity: {Quantity}, State: {State}, ParentId: {ParentOrderId},  Strategy: {Strategy}, PendingQuantity: {PendingQuantity}, FilledQuantity: {FilledQuantity}, CancelState: {CancelState}";
        }
    }
}