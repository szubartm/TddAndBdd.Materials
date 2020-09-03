using System.Collections.Generic;
using OrderManagementSystem;
using TradingSystemConsole.Parsers;

namespace TradingSystemConsole.Commands
{
    public class RejectOrder:ICommand
    {
        private readonly IOrderManager _orderManager;
        private readonly IOutputWriter _outputWriter;

        public RejectOrder(
            IOrderManager orderManager,
            IOutputWriter outputWriter)
        {
            _orderManager = orderManager;
            _outputWriter = outputWriter;
        }

        public string Name => "REJECT_ORDER";
        public void Execute(IEnumerable<string> input)
        {
            var parser = new RejectOrderParser();
            parser.Parse(input);
            var result = _orderManager.Reject(parser.Id);

            _outputWriter.Write(result.ToString());
        }
    }
}