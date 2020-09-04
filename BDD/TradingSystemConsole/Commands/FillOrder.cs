using System.Collections.Generic;
using OrderManagementSystem;
using TradingSystemConsole.Parsers;

namespace TradingSystemConsole.Commands
{
    public class FillOrder :ICommand
    {
        private readonly IOrderManager _orderManager;
        private readonly IOutputWriter _outputWriter;

        public FillOrder(
            IOrderManager orderManager,
            IOutputWriter outputWriter)
        {
            _orderManager = orderManager;
            _outputWriter = outputWriter;
        }

        public string Name => "FILL_ORDER";
        public void Execute(IEnumerable<string> input)
        {
            var parser = new FillOrderParser();
            parser.Parse(input);
            var result = _orderManager.Fill(parser.Id, parser.Quantity);

            _outputWriter.Write(result.ToString());
        }
    }
}