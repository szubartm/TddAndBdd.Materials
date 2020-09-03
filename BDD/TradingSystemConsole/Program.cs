using System;
using System.Collections.Generic;
using System.Linq;
using TradingSystemConsole.Commands;

namespace TradingSystemConsole
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var tradingEnvironment = new TradingEnvironment();
            Console.WriteLine("Welcome in Trading Environment!");

            var outputWriter = new ConsoleOutputWriter();

            var commands = RegisterCommands(
                new InsertOrder(tradingEnvironment.OrderManager, outputWriter), 
                new AcknowledgeOrder(tradingEnvironment.OrderManager, outputWriter),
                new CancelOrder(tradingEnvironment.OrderManager, outputWriter),
                new FillOrder(tradingEnvironment.OrderManager, outputWriter),
                new GetAllOrders(tradingEnvironment.OrderManager, outputWriter),
                new GetChildOrderByParentId(tradingEnvironment.OrderManager, outputWriter),
                new GetOrderById(tradingEnvironment.OrderManager, outputWriter),
                new InsertMarketData(tradingEnvironment.MarketDataSimulator),
                new RejectOrder(tradingEnvironment.OrderManager, outputWriter),
                new SetTime(tradingEnvironment.TimeSimulator),
                new UpdateOrder(tradingEnvironment.OrderManager, outputWriter),
                new UpdateTime(tradingEnvironment.TimeSimulator)
          );

            string[] input;
            do
            {
                input = Console.ReadLine().Split(' ');
            } while (ExecuteCommand(commands, input));

            Console.WriteLine("Goodbye!");
        }

        private static IReadOnlyDictionary<string, ICommand> RegisterCommands(
            params ICommand[] commands)
            => commands.ToDictionary(c => c.Name, c => c);

        private static bool ExecuteCommand(IReadOnlyDictionary<string, ICommand> commands, IReadOnlyList<string> input)
        {
            var commandName = input[0];
            if (commandName.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (commands.TryGetValue(commandName, out var command))
            {
                command.Execute(input);
            }
            else
            {
                Console.WriteLine($"Command [{commandName}] invalid or not supported.");
            }

            return true;
        }
    }
}