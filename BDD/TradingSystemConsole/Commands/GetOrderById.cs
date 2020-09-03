using System.Collections.Generic;
using OrderManagementSystem;
using TradingSystemConsole.Parsers;

namespace TradingSystemConsole.Commands
{
    public class GetOrderById:ICommand
    {
        private readonly IOrderManager _orderManager;
        private readonly IOutputWriter _outputWriter;

        public GetOrderById(
            IOrderManager orderManager,
            IOutputWriter outputWriter)
        {
            _orderManager = orderManager;
            _outputWriter = outputWriter;
        }

        public string Name => "GET_ORDER_BY_ID";
        public void Execute(IEnumerable<string> input)
        {
            var parser = new GetOrderByIdParser();
            parser.Parse(input);
            var result = _orderManager.GetById(parser.Id);

            _outputWriter.Write(result.ToString());
        }
    }
}