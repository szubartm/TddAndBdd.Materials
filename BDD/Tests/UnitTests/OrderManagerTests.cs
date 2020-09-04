using System;
using System.Linq;
using Common;
using NUnit.Framework;
using OrderManagementSystem;

namespace UnitTests
{
    public class OrderManagerTests
    {
        private OmsOrder GetOrder() => new OmsOrder()
        {
            Id = null,
            ParentOrderId = null,
            //PendingQuantity = 0,
            Price = 10,
            Quantity = 100,
            Side = Side.Buy,
            State = State.UNACK,
            Symbol = "AZN.L",
            TimeInForce = TimeInForce.Day,
            Timestamp = DateTime.Now,
            Strategy = StrategyTypes.Sniper
        };

        [Test]
        public void InsertOrderShouldReturnNewId()
        {
            var orderManager = new OrderManager();
            var order = GetOrder();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            Assert.IsNotNull(id);
        }

        [Test]
        public void GetByIdShouldThrowExceptionIfOrderNotFound()
        {
            var orderManager = new OrderManager();
            var result = orderManager.GetById("fakeId");
            Assert.Null(result);
        }

        [Test]
        public void GetByIdShouldReturnLatestOrderSnapshot()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Acknowledge(id);
            orderManager.Update(id, 10, 1);
            var result = orderManager.GetById(id);
            Assert.AreEqual(result.Price, 10);
        }

        [Test]
        public void GetAllChildsShouldReturnEmptyListIfNoChildsFound()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            var result = orderManager.GetAllChilds(id);
            Assert.False(result.Any());
        }

        [Test]
        public void GetAllChildsShouldReturnListWithLatestChildOrdersSnapshot()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            var child1 = new OmsOrder() {ParentOrderId = id};
            var child2 = new OmsOrder() {ParentOrderId = id};
            var child3 = new OmsOrder() {ParentOrderId = id};

            orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce, id,
                order.Strategy);
            orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce, id,
                order.Strategy);
            orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce, id,
                order.Strategy);
            var result = orderManager.GetAllChilds(id);
            Assert.True(result.Count() == 3);
        }

        [Test]
        public void GetAllShouldReturnEmptyListIfOrderListIsEmpty()
        {
            var orderManager = new OrderManager();
            var result = orderManager.GetAll();
            Assert.False(result.Any());
        }

        [Test]
        public void GetAllShouldReturnListWithLatestOrdersSnapshot()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Update(id, 999, order.Quantity);
            var id2 = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            var result = orderManager.GetAll();
            Assert.True(result.Count() == 2);
            Assert.AreEqual(result.First(x => x.Id == id).Price, 999);
            Assert.AreEqual(result.First(x => x.Id == id2).Price, order.Price);
        }

        [Test]
        public void AcknowledgeShouldThrowExceptionIfOrderNotFound()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            var result = orderManager.Acknowledge("fakeId");
            Assert.False(result);
        }

        [Test]
        public void AcknowledgeShouldAcknowledgeOrder()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Acknowledge(id);
            var result = orderManager.GetById(id);
            Assert.AreEqual(State.LIVE, result.State);
        }


        [Test]
        public void UpdateShouldThrowExceptionIfOrderNotFound()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            var result = orderManager.Update("fakeId", 0, 0);
            Assert.False(result);
        }

        [Test]
        public void UpdateShouldUpdateOrder()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Update(id, 1, order.Quantity);
            var result = orderManager.GetById(id);
            Assert.AreEqual(result.Price, 1);
            Assert.AreEqual(result.Quantity, order.Quantity);
        }

        [Test]
        public void UpdateShouldThrowExceptionIfOrderNotInAllowedToUpdateState()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Cancel(id);
            orderManager.Acknowledge(id);
            var result = orderManager.Update(id, 10, 10);
            Assert.False(result);
        }

        [Test]
        public void FillShouldThrowExceptionIfOrderNotFound()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Acknowledge(id);
            Assert.False(orderManager.Fill("fakeId", 100));
        }

        [Test]
        public void FillShouldThrowIfParentOrder()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Acknowledge(id);
            Assert.Throws<NotSupportedException>(() => orderManager.Fill(id, 90));
        }

        [Test]
        public void FillShouldFillParentOrder()
        {
            var order = GetOrder();
            var expectedQty = order.Quantity - 90;
            var orderManager = new OrderManager();
            var parentId = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                parentId, order.Strategy);
            orderManager.Acknowledge(parentId);
            orderManager.Acknowledge(id);
            orderManager.Fill(id, 90);
            var parent = orderManager.GetById(parentId);
            Assert.True(expectedQty == parent.PendingQuantity);
            var child = orderManager.GetById(id);
            Assert.True(expectedQty == child.PendingQuantity);
        }

        [Test]
        public void FillShouldThrowExceptionIfOrderNotInAckOrPfillState()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            var result = orderManager.Fill(id, 90);
            Assert.False(result);
        }

        [Test]
        public void CancelShouldThrowExceptionIfOrderNotFound()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Acknowledge(id);
            Assert.False(orderManager.Cancel("fakeId"));
        }

        [Test]
        public void CancelShouldCancelOrder()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Acknowledge(id);
            orderManager.Cancel(id);
            orderManager.Acknowledge(id);
            var result = orderManager.GetById(id);
            Assert.True(State.CANCELLED == result.State);
        }

        [Test]
        public void CancelShouldThrowExceptionIfOrderInNotAllowedToBeCancelledState()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Acknowledge(id);
            orderManager.Reject(id);
            var result = orderManager.Cancel(id);
            Assert.False(result);
        }

        [Test]
        public void RejectShouldThrowExceptionIfOrderNotFound()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Acknowledge(id);
            var result = orderManager.Reject("fakeId");
            Assert.False(result);
        }

        [Test]
        public void RejectShouldRejectOrder()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Acknowledge(id);
            orderManager.Reject(id);
            var result = orderManager.GetById(id);
            Assert.True(State.REJECTED == result.State);
        }

        [Test]
        public void RejectShouldThrowExceptionIfOrderInNotAllowedToBeCRejectedState()
        {
            var order = GetOrder();
            var orderManager = new OrderManager();
            var id = orderManager.Insert(order.Symbol, order.Side, order.Price, order.Quantity, order.TimeInForce,
                order.ParentOrderId, order.Strategy);
            orderManager.Acknowledge(id);
            orderManager.Cancel(id);
            orderManager.Acknowledge(id);
            var result = orderManager.Reject(id);
            Assert.False(result);
        }
    }
}