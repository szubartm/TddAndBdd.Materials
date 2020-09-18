using System.Linq;
using System.Threading.Tasks;
using Behavioral.Tools.ScenarioHelpers;
using Common;
using NUnit.Framework;
using TechTalk.SpecFlow;

namespace Behavioral.Steps
{
    [Binding]
    public class OrderAssertionsSteps
    {
        private readonly ScenarioHelper _scenarioHelper;

        public OrderAssertionsSteps(ScenarioHelper scenarioHelper)
        {
            _scenarioHelper = scenarioHelper;
        }
        [Given(@"order is accepted")]
        [When(@"order is accepted")]
        [Then(@"order is accepted")]
        [Given(@"zlecenie zostało zaakceptowane")]
        [When(@"zlecenie zostało zaakceptowane")]
        [Then(@"zlecenie zostało zaakceptowane")]
        public async Task OrderIsAccepted()
        {
            var order = await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.GetById(_scenarioHelper.OrderId);
            Assert.IsNotNull(order);
            Assert.IsTrue(order.State == State.LIVE);
        }

        [Then(@"order is rejected")]
        [Then(@"zlecenie zostało odrzucone")]
        public async Task OrderIsRejected()
        {
            var order = await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.GetById(_scenarioHelper.OrderId);
            Assert.IsNotNull(order);
            Assert.IsTrue(order.State == State.REJECTED);
        }

        [Then(@"order is in filled state")]
        public async Task OrderIsInFilledState()
        {
            var order = await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.GetById(_scenarioHelper.OrderId); 
            Assert.IsNotNull(order);
            Assert.IsTrue(order.State == State.FILLED);
        }

        [Then(@"child order was sent")]
        [Then(@"podzlecenie zostało wysłane")]
        public async Task ChildOrderWasSent()
        {
            var child = await _scenarioHelper.GetLastChild();
            Assert.IsTrue(child.ParentOrderId == _scenarioHelper.OrderId);
        }

        [Then(@"child order should not be sent")]
        public async Task ChildOrderShouldNotBeSent()
        {
            var childCount = (await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.GetAllChilds(_scenarioHelper.OrderId)).Count();
            Assert.True(childCount == _scenarioHelper.ChildOrders.Count());
        }

        [Then(@"child order is cancelled")]
        public async Task ChildOrderIsCancelled()
        {
            var child = await _scenarioHelper.GetLastChild();
            Assert.True(child.CancelState == State.PCANCEL);
            await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.Acknowledge(child.Id);
        }
    }
}