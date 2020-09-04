using System;

namespace TradingSystem.Reporting
{
    public class ConsoleReporter : IReporter
    {
        public void Send(Report report)
        {
            Console.WriteLine($"{report.TimeStamp}: {report.Message}");
        }
    }

    public class EmptyReporter : IReporter
    {
        public void Send(Report report)
        {
        }
    }

    public interface IReporter
    {
        void Send(Report report);
    }
}