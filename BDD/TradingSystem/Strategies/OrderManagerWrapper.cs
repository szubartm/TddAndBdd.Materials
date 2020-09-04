using Common;
using OrderManagementSystem;

namespace TradingSystem.Strategies
{
    public class OrderManagerWrapper
    {
        private readonly IOrderManager _orderManager;

        public OrderManagerWrapper(
            IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        public bool Acknowledge(TsOrder order)
         => _orderManager.Acknowledge(order.Id);

        public bool Reject(TsOrder order)
            => _orderManager.Reject(order.Id);

        public TsOrder SendChild(TsOrder order)
        {
            return SendChild(order, order.TimeInForce);
        }

        public TsOrder SendChild(TsOrder order, TimeInForce timeInForce)
        {
            var childId = _orderManager.Insert(order.Symbol, order.Side, order.Price, 
                order.PendingQuantity, timeInForce, order.Id, order.Strategy);
            var omsOrder = _orderManager.GetById(childId);
            return TsOrder.From(omsOrder);
        }

        public TsOrder ModifyChild(TsOrder order, decimal price, int quantity)
        {
            var childId = _orderManager.Insert(order.Symbol, order.Side, price, 
                quantity, order.TimeInForce, order.Id, order.Strategy);
            var omsOrder = _orderManager.GetById(childId);
            return TsOrder.From(omsOrder);
        }

        public bool CancelChild(TsOrder order)
            => _orderManager.Cancel(order.Id);

        public TsOrder Refresh(TsOrder order)
        {
            var omsOrder = _orderManager.GetById(order.Id);
            return TsOrder.From(omsOrder);
        }
    }
}