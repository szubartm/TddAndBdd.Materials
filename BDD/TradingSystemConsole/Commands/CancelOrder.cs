using System.Collections.Generic;
using OrderManagementSystem;
using TradingSystemConsole.Parsers;

namespace TradingSystemConsole.Commands
{
    public class CancelOrder : ICommand
    {
        private readonly IOrderManager _orderManager;
        private readonly IOutputWriter _outputWriter;

        public CancelOrder(
            IOrderManager orderManager,
            IOutputWriter outputWriter)
        {
            _orderManager = orderManager;
            _outputWriter = outputWriter;
        }

        public string Name => "CANCEL_ORDER";
        public void Execute(IEnumerable<string> input)
        {
            var parser = new CancelOrderParser();
            parser.Parse(input);
            var result = _orderManager.Cancel(parser.Id);

            _outputWriter.Write(result.ToString());
        }
    }
}