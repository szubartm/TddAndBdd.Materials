using System.Collections.Generic;
using Common;

namespace OrderManagementSystem
{
    public interface IParentOrder
    {
        IReadOnlyList<IOrder> Children { get; }
    }
}