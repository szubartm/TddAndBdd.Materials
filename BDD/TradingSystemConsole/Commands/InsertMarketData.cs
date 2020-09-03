using System.Collections.Generic;
using DataProviderSystem;
using OrderManagementSystem;
using TradingSystemConsole.Parsers;

namespace TradingSystemConsole.Commands
{
    public class InsertMarketData:ICommand
    {
        private readonly IMarketDataSimulator _dataSimulator;

        public InsertMarketData(
            IMarketDataSimulator dataSimulator)
        {
            _dataSimulator = dataSimulator;
        }

        public string Name => "INSERT_MARKET_DATA";
        public void Execute(IEnumerable<string> input)
        {
            var parser = new InsertMarketDataParser();
            parser.Parse(input);
            _dataSimulator.InsertNewMarketData(parser.Symbol, parser.Bid, parser.Ask, parser.BidSize,
                parser.AskSize);
        }
    }
}