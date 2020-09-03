using System.Collections.Generic;

namespace TradingSystemConsole.Commands
{
    public interface ICommand
    {
        string Name { get; }
        void Execute(IEnumerable<string> input);
    }
}