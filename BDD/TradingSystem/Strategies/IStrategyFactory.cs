using Common;
using OrderManagementSystem;
using TradingSystem.Strategies.DayIn;

namespace TradingSystem.Strategies
{
    public interface IStrategyFactory
    {
        IStrategy CreateStrategy(IOrder order, TradingSessionType tradingSessionType, IOrderManager orderManager);
    }

    public class SimpleStrategyFactory : IStrategyFactory
    {
        public IStrategy CreateStrategy(IOrder order, TradingSessionType tradingSessionType, IOrderManager orderManager)
        {
            var tsOrder = TsOrder.From(order);
            if(tsOrder.Strategy == StrategyTypes.Day)
                return new DayInAuction(tsOrder, tradingSessionType, orderManager);
            else
                return new Sniper.Sniper(orderManager, order);
        }
    }
}