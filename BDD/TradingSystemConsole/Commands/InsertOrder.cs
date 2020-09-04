using System;
using System.Collections.Generic;
using OrderManagementSystem;
using TradingSystemConsole.Parsers;

namespace TradingSystemConsole.Commands
{
    public class InsertOrder : ICommand
    {
        private readonly IOrderManager _orderManager;
        private readonly IOutputWriter _outputWriter;

        public InsertOrder(
            IOrderManager orderManager,
            IOutputWriter outputWriter)
        {
            _orderManager = orderManager;
            _outputWriter = outputWriter;
        }

        public string Name => "INSERT_ORDER";
        public void Execute(IEnumerable<string> input)
        {
            var parser = new InsertOrderParser();
            parser.Parse(input);

            var id = _orderManager.Insert(parser.Symbol, parser.Side, parser.Price, parser.Quantity,
                parser.TimeInForce, null, parser.Strategy);

            _outputWriter.Write(id);
        }
    }
}
