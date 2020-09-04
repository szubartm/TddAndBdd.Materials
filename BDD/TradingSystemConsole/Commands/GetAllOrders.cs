using System.Collections.Generic;
using OrderManagementSystem;
using TradingSystemConsole.Parsers;

namespace TradingSystemConsole.Commands
{
    public class GetAllOrders :ICommand
    {
        private readonly IOrderManager _orderManager;
        private readonly IOutputWriter _outputWriter;

        public GetAllOrders(
            IOrderManager orderManager,
            IOutputWriter outputWriter)
        {
            _orderManager = orderManager;
            _outputWriter = outputWriter;
        }

        public string Name => "GET_ALL_ORDERS";
        public void Execute(IEnumerable<string> input)
        {
            var parser = new GetAllOrdersParser();
            parser.Parse(input);
            var result = _orderManager.GetAll();

            foreach (var order in result)
            {
                _outputWriter.Write(order.ToString());
            }
        }
    }
}