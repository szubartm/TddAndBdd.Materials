using System.Threading.Tasks;
using Behavioral.Tools.PlainTypes;
using Behavioral.Tools.ScenarioHelpers;
using Common;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Behavioral.Steps
{
    [Binding]
    public class SendingOrderSteps
    {
        private readonly ScenarioHelper _scenarioHelper;

        public SendingOrderSteps(ScenarioHelper scenarioHelper)
        {
            _scenarioHelper = scenarioHelper;
        }
        [Given(@"new order is received")]
        [When(@"new order is received")]
        [When(@"nowe zlecenie zostało otrzymane")]
        public Task NewOrderIsReceived() => InsertOrder(StrategyTypes.Day);

        private async Task InsertOrder(StrategyTypes strategyType)
        {
            _scenarioHelper.OrderId = await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.Insert("AZN.L", Side.Buy, 100, 10000,
                TimeInForce.Day, null, strategyType);
        }

        [Given(@"new order is received with params")]
        [When(@"new order is received with params")]
        [When(@"nowe zlecenie zostało otrzymane z parametrami")]
        public async Task NewOrderIsReceivedWithParams(Table table)
        {
            var order = table.CreateInstance<Order>();
            _scenarioHelper.OrderId = await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.Insert(order.Symbol, order.Side, order.Price, order.Quantity,
                order.TimeInForce, order.ParentOrderId, order.StrategyType);
        }

        [Scope(Feature = "SniperStrategyFeatures")]
        [Given(@"new order is received")]
        [When(@"new order is received")]
        public Task NewSniperOrderIsReceived()
            => InsertOrder(StrategyTypes.Sniper);

    }
}