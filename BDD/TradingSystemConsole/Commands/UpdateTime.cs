using System.Collections.Generic;
using OrderManagementSystem;
using TradingSystem;
using TradingSystemConsole.Parsers;

namespace TradingSystemConsole.Commands
{
    public class UpdateTime:ICommand
    {
        private readonly ITimeSimulator _timeSimulator;

        public UpdateTime(
            ITimeSimulator timeSimulator)
        {
            _timeSimulator = timeSimulator;
        }

        public string Name => "UPDATE_TIME";
        public void Execute(IEnumerable<string> input)
        {
            var parser = new UpdateTimeParser();
            parser.Parse(input);
            _timeSimulator.UpdateTimeBy(parser.Time);
        }
    }
}