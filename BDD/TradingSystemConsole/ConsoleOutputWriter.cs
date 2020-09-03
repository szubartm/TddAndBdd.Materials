using System;

namespace TradingSystemConsole
{
    public class ConsoleOutputWriter : IOutputWriter
    {
        public void Write(string value)
        {
            Console.WriteLine(value);
        }
    }
}