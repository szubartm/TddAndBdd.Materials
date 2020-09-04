//using System;
//using Common;
//using Moq;
//using NUnit.Framework;
//using OrderManagementSystem;
//using TradingSystem;
//using TradingSystem.DayIn2;

//namespace UnitTests
//{
//    public class TradingSystemTests
//    {
//        [Test]
//        public void Test1()
//        {
//            var orderManager = new OrderManager();
//            var reporterMock = new Mock<IReporter>();
//            var strategyMock = Mock.Of<IStrategy>();
//            var strategyFactory = new Mock<IStrategyFactory>();
//            var order = new TsOrder
//            {
//                Id = "1", Symbol = "AZN.L", Side = Side.Buy, Price = 10, Quantity = 100, TimeInForce = TimeInForce.Day,
//                Strategy = StrategyTypes.Day
//            };
//            strategyFactory
//                .Setup(s => s.CreateStrategy(It.IsAny<IOrder>(), It.IsAny<TradingSessionType>(), It.IsAny<DateTime>()))
//                .Returns(new DayInAuction(order, TradingSessionType.ContinuesTrading, orderManager));
//            var tradingSystem = new TradingSystem.TradingSystem(1, orderManager, null, strategyFactory.Object,
//                reporterMock.Object, new TimeSimulator(), new SettingsValidator());
//            var id = orderManager.Insert("azn", Side.Buy, 10, 100, TimeInForce.Day, null, StrategyTypes.Day);
//            reporterMock.Verify(r => r.Send(It.IsAny<OrderReceivedReport>()));
//            orderManager.Acknowledge(id);

//            Assert.Pass();
//        }
//    }
//}