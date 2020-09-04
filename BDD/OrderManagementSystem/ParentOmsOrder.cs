using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace OrderManagementSystem
{
    public class ParentOmsOrder : OmsOrderBase
    {
        private readonly Dictionary<string, IOrder> _childOrders = new Dictionary<string, IOrder>();
        private State _state;
        public override int PendingQuantity => _childOrders.Any() ? Quantity - _childOrders.Sum(x => x.Value.FilledQuantity) : Quantity;

        public override int FilledQuantity => _childOrders.Any() ? _childOrders.Sum(x => x.Value.FilledQuantity) : 0;

        public override State State
        {
            get
            {
                if (PendingQuantity == 0)
                    _state = State.FILLED;
                return _state;
            }
            set { _state = value; }
        }



        public override void AddChild(IOrder child)
        {
            _childOrders.Add(child.Id, child);
        }
         
        public override IEnumerable<IOrder> GetAllChilds()
        {
            return _childOrders.Select(x => x.Value).ToArray();
        }
    }
}