using System;
using Common;
using NUnit.Framework;
using OrderManagementSystem;
using TradingSystem;
using TradingSystem.Strategies.Sniper;

namespace UnitTests
{
    public class SniperStrategyTests
    {
        [Test]
        public void SniperShouldRejectOrderExceedingQuantityLimits()
        {
            var orderManager = new OrderManager();
            var orderId = orderManager.Insert("azn.l", Side.Buy, 100, 91000, TimeInForce.Day,
                null, StrategyTypes.Sniper);
            var omsOrder = orderManager.GetById(orderId);
            var order = TsOrder.From(omsOrder);
            var sniper = new Sniper(orderManager, order);
            sniper.StrategyFinishedProcessing += (string tmp) => { };
            
            sniper.Start();
            var orderSnap = orderManager.GetById("1");
            Assert.True(orderSnap.State == State.REJECTED);
        }
    }
}