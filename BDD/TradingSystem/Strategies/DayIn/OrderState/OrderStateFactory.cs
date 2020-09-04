using Common;
using TradingSystem.Strategies.DayIn.AuctionStates;

namespace TradingSystem.Strategies.DayIn.OrderState
{
    internal static class OrderStateFactory
    {
        public static ITsOrderState Create(OrderChangeType changeType, TsOrder order, IAuctionState auctionState)
        {
            if (order.Id == auctionState.Context.ParentOrder.Id && changeType == OrderChangeType.Cancellation)
                return new ParentOrderCancelled(auctionState);

            if (order.Id == auctionState.Context.ParentOrder.Id && changeType == OrderChangeType.Modification)
                return new ParentOrderModified(order, auctionState);

            if (order.Id == auctionState.Context.ChildOrder?.Id && changeType == OrderChangeType.Rejection)
                return new ChildOrderRejected(auctionState);

            if (order.Id == auctionState.Context.ChildOrder?.Id && changeType == OrderChangeType.Fill)
                return new ChildOrderModified(order, auctionState);

            return new NotImportantChange(auctionState);
        }
    }
}