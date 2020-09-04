using System.Collections.Generic;
using OrderManagementSystem;
using TradingSystemConsole.Parsers;

namespace TradingSystemConsole.Commands
{
    public class AcknowledgeOrder: ICommand
    {
        private readonly IOrderManager _orderManager;
        private readonly IOutputWriter _outputWriter;

        public AcknowledgeOrder(
            IOrderManager orderManager,
            IOutputWriter outputWriter)
        {
            _orderManager = orderManager;
            _outputWriter = outputWriter;
        }

        public string Name => "ACKNOWLEDGE_ORDER";
        public void Execute(IEnumerable<string> input)
        {
            var parser = new AcknowledgeOrderParser();
            parser.Parse(input);
            var result = _orderManager.Acknowledge(parser.Id);

            _outputWriter.Write(result.ToString());
        }
    }
}