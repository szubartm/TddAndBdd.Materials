using System.Linq;
using Common;

namespace TradingSystem.Validation
{
    public interface IOrderValidator
    {
        bool Validate(IOrder order);
    }

    public class SettingsValidator : IOrderValidator
    {
        public bool Validate(IOrder order)
        {
            if (!Settings.AllowedInstruments.Contains(order.Symbol))
                return false;
            return true;
        }
    }
}