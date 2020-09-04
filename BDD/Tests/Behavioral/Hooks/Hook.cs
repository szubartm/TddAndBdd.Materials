using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Behavioral.Tools;
using Behavioral.Tools.SeparateProcess;
using Behavioral.Tools.ScenarioHelpers;
using Common;
using Microsoft.Extensions.Configuration;
using TechTalk.SpecFlow;

namespace Behavioral.Hooks
{
    [Binding]
    public class Hooks
    {
        private static SutLifetimeController _sutLifetimeController;
         
        public static SimulationContext SimulationContext { get; private set; }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            _sutLifetimeController = new SutLifetimeController();
            _sutLifetimeController.StartSut(config["sutFilePath"]);
            SimulationContext = new SimulationContext(config["sutWebApiUri"]);
        }

        [AfterScenario()]
        public static async Task AfterScenario()
        {
            await SimulationContext.MarketDataServiceSimulator.ClearMarketData();
            ScenarioHelper.Instance.ChildOrders = new List<IOrder>();
            ScenarioHelper.Instance.OrderId = null;
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _sutLifetimeController.Dispose();
        }
    }
}