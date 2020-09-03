using System.Collections.Generic;
using OrderManagementSystem;
using TradingSystem;
using TradingSystemConsole.Parsers;

namespace TradingSystemConsole.Commands
{
    public class SetTime:ICommand
    {
        private readonly ITimeSimulator _timeSimulator;

        public SetTime(
            ITimeSimulator timeSimulator)
        {
            _timeSimulator = timeSimulator;
        }

        public string Name => "SET_TIME";
        public void Execute(IEnumerable<string> input)
        {
            var parser = new SetTimeParser();
            parser.Parse(input);
            _timeSimulator.SetTimeTo(parser.Time);
        }
    }
}