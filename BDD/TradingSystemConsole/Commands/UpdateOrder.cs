using System.Collections.Generic;
using OrderManagementSystem;
using TradingSystemConsole.Parsers;

namespace TradingSystemConsole.Commands
{
    public class UpdateOrder : ICommand
    {
        private readonly IOrderManager _orderManager;
        private readonly IOutputWriter _outputWriter;

        public UpdateOrder(
            IOrderManager orderManager,
            IOutputWriter outputWriter)
        {
            _orderManager = orderManager;
            _outputWriter = outputWriter;
        }

        public string Name => "UPDATE_ORDER";
        public void Execute(IEnumerable<string> input)
        {
            var parser = new UpdateOrderParser();
            parser.Parse(input);
            var updateResult = _orderManager.Update(parser.Id, parser.Price, parser.Quantity);

            _outputWriter.Write(updateResult.ToString());
        }
    }
}