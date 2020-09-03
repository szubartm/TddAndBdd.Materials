using System;
using System.Collections.Generic;
using Common;

namespace OrderManagementSystem
{
    public class OmsOrder : OmsOrderBase
    {
        public override int PendingQuantity => Quantity - FilledQuantity;

        public override void AddChild(IOrder child)
        {
            throw new NotSupportedException();
        }

        public override IEnumerable<IOrder> GetAllChilds()
        {
            throw new NotSupportedException();
        }
    }
}