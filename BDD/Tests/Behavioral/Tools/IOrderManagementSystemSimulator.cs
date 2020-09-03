using System.Collections.Generic;
using System.Threading.Tasks;
using Common;

namespace Behavioral.Tools
{
    public interface IOrderManagementSystemSimulator
    {
        Task<bool> Acknowledge(string id);
        Task<bool> Cancel(string id);
        Task<bool> Fill(string id, int quantity);
        Task<IEnumerable<IOrder>> GetAll();
        Task<IEnumerable<IOrder>> GetAllChilds(string id);
        Task<IOrder> GetById(string id);

        Task<string> Insert(string symbol, Side side, decimal price, int quantity, TimeInForce timeInForce,
            string parentOrderId, StrategyTypes strategy);

        Task<bool> Reject(string id);
        Task<bool> Update(string id, decimal newPrice, int newQuantity);
    }
}