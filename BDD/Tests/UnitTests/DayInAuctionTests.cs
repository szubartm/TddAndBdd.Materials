using System;
using Common;
using DataProviderSystem;
using NUnit.Framework;
using OrderManagementSystem;
using TradingSystem;
using TradingSystem.Strategies.DayIn;

namespace UnitTests
{
    public class DayInAuctionTests
    {
        [Test]
        public void DayInAuctionShouldPlaceNewOrderWhenMarketMovesToAuction()
        {
            var orderManager = new OrderManager();
            var orderId = orderManager.Insert("azn.l", Side.Buy, 100, 2000, TimeInForce.Day,
                null, StrategyTypes.Day);
            var omsOrder = orderManager.GetById(orderId);
            var order = TsOrder.From(omsOrder);
            var dayInAuction = new DayInAuction(order, TradingSessionType.ContinuesTrading, orderManager);
            orderManager.OrderChanged += dayInAuction.onOrderChanged;
            dayInAuction.Start();

            dayInAuction.onTradingSessionChanged(TradingSessionType.OpenAuction);

            var child1 = orderManager.GetById("2");
            Assert.NotNull(child1);

            dayInAuction.onTradingSessionChanged(TradingSessionType.IntradayAuction);
        }

        [Test]
        public void DayInAuctionShouldCancelChildIfMarketMovesOutOfAuction()
        {
            // Arrange
            var orderManager = new OrderManager();
            var orderId = orderManager.Insert("azn.l", Side.Buy, 100, 2000, TimeInForce.Day,
                null, StrategyTypes.Day);
            var omsOrder = orderManager.GetById(orderId);
            var order = TsOrder.From(omsOrder);
            var dayInAuction = new DayInAuction(order, TradingSessionType.ContinuesTrading, orderManager);
            orderManager.OrderChanged += dayInAuction.onOrderChanged;

            // act
            dayInAuction.Start();

            dayInAuction.onTradingSessionChanged(TradingSessionType.OpenAuction);

            var child1 = orderManager.GetById("2");

            dayInAuction.onTradingSessionChanged(TradingSessionType.IntradayAuction);
            orderManager.Acknowledge("2");
            dayInAuction.onTradingSessionChanged(TradingSessionType.CloseAuction);

            var cancelChild1 = orderManager.GetById("2");

            // assert
            Assert.NotNull(child1);
            Assert.True(cancelChild1.CancelState == State.PCANCEL);
        }

        [Test]
        public void DayInAuctionShouldDoNothingUntilChildFullFilledOrMarketMovesOutOfAuction()
        {
            var orderManager = new OrderManager();
            var orderId = orderManager.Insert("azn.l", Side.Buy, 100, 2000, TimeInForce.Day,
                null, StrategyTypes.Day);
            var omsOrder = orderManager.GetById(orderId);
            var order = TsOrder.From(omsOrder);
            var dayInAuction = new DayInAuction(order, TradingSessionType.ContinuesTrading, orderManager);
            orderManager.OrderChanged += dayInAuction.onOrderChanged;
            dayInAuction.Start();

            dayInAuction.onTradingSessionChanged(TradingSessionType.OpenAuction);

            var child1 = orderManager.GetById("2");
            Assert.NotNull(child1);

            dayInAuction.onTradingSessionChanged(TradingSessionType.IntradayAuction);
            orderManager.Acknowledge("2");
            orderManager.Fill("2", 100);
            orderManager.Fill("2", 250);

            var child1Snap2 = orderManager.GetById("2");
            Assert.True(child1Snap2.PendingQuantity == order.Quantity - 350);
            Assert.True(child1Snap2.State == State.PFILLED);

            dayInAuction.onTradingSessionChanged(TradingSessionType.ContinuesTrading);
            var child1Snap3 = orderManager.GetById("2");
            Assert.True(child1Snap3.CancelState == State.PCANCEL);
            orderManager.Acknowledge("2");

            var child2 = orderManager.GetById("3");
            Assert.Null(child2);
            dayInAuction.onTradingSessionChanged(TradingSessionType.OpenAuction);
            orderManager.Acknowledge("3");

            var child2Snap2 = orderManager.GetById("3");
            Assert.True(child2Snap2.PendingQuantity == order.Quantity - 350);
            Assert.True(child2Snap2.State == State.LIVE);
        }

        [Test]
        public void DayInAuctionShouldDoNothingUntilChildFullFilledOrMarketMovesOutOfAuction2()
        {
            var orderManager = new OrderManager();
            var orderId = orderManager.Insert("azn.l", Side.Buy, 100, 2000, TimeInForce.Day,
                null, StrategyTypes.Day);
            var omsOrder = orderManager.GetById(orderId);
            var order = TsOrder.From(omsOrder);
            var dayInAuction = new DayInAuction(order, TradingSessionType.ContinuesTrading, orderManager);
            orderManager.OrderChanged += dayInAuction.onOrderChanged;
            dayInAuction.Start();

            dayInAuction.onTradingSessionChanged(TradingSessionType.OpenAuction);

            var child1 = orderManager.GetById("2");
            Assert.NotNull(child1);

            dayInAuction.onTradingSessionChanged(TradingSessionType.IntradayAuction);
            orderManager.Acknowledge("2");
            orderManager.Fill("2", 100);
            orderManager.Fill("2", 250);

            var child1Snap2 = orderManager.GetById("2");
            Assert.True(child1Snap2.PendingQuantity == order.Quantity - 350);
            Assert.True(child1Snap2.State == State.PFILLED);

            orderManager.Fill("2", order.Quantity - 350);
            var child1Snap3 = orderManager.GetById("2");
            Assert.True(child1Snap3.State == State.FILLED);

            dayInAuction.onTradingSessionChanged(TradingSessionType.CloseAuction);
            var child1Snap4 = orderManager.GetById("2");
            Assert.True(child1Snap4.State == State.FILLED);
        }

        [Test]
        public void DayInAuctionShouldIgnoreMarketData()
        {
            var orderManager = new OrderManager();
            var orderId = orderManager.Insert("azn.l", Side.Buy, 100, 2000, TimeInForce.Day,
                null, StrategyTypes.Day);
            var omsOrder = orderManager.GetById(orderId);
            var order = TsOrder.From(omsOrder);
            var dayInAuction = new DayInAuction(order, TradingSessionType.IntradayAuction, orderManager);
            orderManager.OrderChanged += dayInAuction.onOrderChanged;
            dayInAuction.Start();

            var child = orderManager.GetById("2");
            Assert.NotNull(child);

            dayInAuction.onNewMarketData(new MarketData("azn.l", 10, 11, 100, 200));
            var child2 = orderManager.GetById("2");
            Assert.AreSame(child, child2);
        }

        [Test]
        public void DayInAuctionShouldIgnoreTimeChange()
        {

            var orderManager = new OrderManager();
            var orderId = orderManager.Insert("azn.l", Side.Buy, 100, 2000, TimeInForce.Day,
                null, StrategyTypes.Day);
            var order = orderManager.GetById(orderId);
            var dayInAuction = new DayInAuction(TsOrder.From(order), TradingSessionType.IntradayAuction, orderManager);
            orderManager.OrderChanged += dayInAuction.onOrderChanged;
            dayInAuction.Start();

            var child = orderManager.GetById("2");
            Assert.NotNull(child);

            dayInAuction.onTimeChanged(DateTime.Now);
            var child2 = orderManager.GetById("2");
            Assert.AreSame(child, child2);
        }

        [Test]
        public void DayInAuctionShouldRejectOrderModificationIfInAuction()
        {
 
            var orderManager = new OrderManager();
            var orderId = orderManager.Insert("azn.l", Side.Buy, 100, 2000, TimeInForce.Day,
                null, StrategyTypes.Day);
            var omsOrder = orderManager.GetById(orderId);
            var order = TsOrder.From(omsOrder);
            var dayInAuction = new DayInAuction(order, TradingSessionType.IntradayAuction, orderManager);
            orderManager.OrderChanged += dayInAuction.onOrderChanged;
            dayInAuction.Start();

            orderManager.Update("1", 120, 250);
            var orderAfterModification = orderManager.GetById("1");
            Assert.True(orderAfterModification.State == State.REJECTED);
        }
    }
}