using System.Collections.Generic;
using Common;

namespace OrderManagementSystem
{
    public delegate void NewOrderInsertedEventHandler(IOrder order);

    public delegate void OrderChangedEventHandler(OrderChangeType changeType, IOrder order);

    public interface IOrderManager
    {
        event NewOrderInsertedEventHandler NewOrderInserted;
        event OrderChangedEventHandler OrderChanged;

        bool Acknowledge(string id);
        bool Cancel(string id);
        bool Fill(string id, int quantity);
        IEnumerable<IOrder> GetAll();
        IEnumerable<IOrder> GetAllChilds(string id);
        IOrder GetById(string id);

        string Insert(string Symbol, Side Side, decimal Price, int Quantity, TimeInForce TimeInForce,
            string ParentOrderId, StrategyTypes Strategy);

        bool Reject(string id);
        bool Update(string id, decimal newPrice, int newQuantity);
    }
}