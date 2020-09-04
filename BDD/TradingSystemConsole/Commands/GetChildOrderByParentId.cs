using System.Collections.Generic;
using OrderManagementSystem;
using TradingSystemConsole.Parsers;

namespace TradingSystemConsole.Commands
{
    public class GetChildOrderByParentId : ICommand
    {
        private readonly IOrderManager _orderManager;
        private readonly IOutputWriter _outputWriter;

        public GetChildOrderByParentId(
            IOrderManager orderManager,
            IOutputWriter outputWriter)
        {
            _orderManager = orderManager;
            _outputWriter = outputWriter;
        }

        public string Name => "GET_CHILD_BY_PARENT_ID_ORDERS";
        public void Execute(IEnumerable<string> input)
        {
            var parser = new GetChildOrderByParentIdParser();
            parser.Parse(input);
            var result = _orderManager.GetAllChilds(parser.Id);

            foreach (var order in result)
            {
                _outputWriter.Write(order.ToString());
            }
        }
    }
}