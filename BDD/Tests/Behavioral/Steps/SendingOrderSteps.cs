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
        [Given(@"new order is received")]
        [When(@"new order is received")]
        public Task NewOrderIsReceived() => InsertOrder(StrategyTypes.Day);

        private async Task InsertOrder(StrategyTypes strategyType)
        {
            ScenarioHelper.Instance.OrderId = await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.Insert("AZN.L", Side.Buy, 100, 10000,
                TimeInForce.Day, null, strategyType);
        }

        [Given(@"new order is received with params")]
        [When(@"new order is received with params")]
        public async Task NewOrderIsReceivedWithParams(Table table)
        {
            var order = table.CreateInstance<Order>();
            ScenarioHelper.Instance.OrderId = await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.Insert(order.Symbol, order.Side, order.Price,order.Quantity,
                order.TimeInForce, order.ParentOrderId, order.StrategyType);
        }

        [Given(@"new sniper order is received")]
        [When(@"new sniper order is received")]
        public Task NewSniperOrderIsReceived()
            => InsertOrder(StrategyTypes.Sniper);

    }
}