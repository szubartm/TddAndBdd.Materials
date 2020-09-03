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

        [Given(@"order is accepted")]
        [When(@"order is accepted")]
        [Then(@"order is accepted")]
        public async Task OrderIsAccepted()
        {
            var order = await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.GetById(ScenarioHelper.Instance.OrderId);
            Assert.IsNotNull(order);
            Assert.IsTrue(order.State == State.LIVE);
        }

        [Then(@"order is rejected")]
        public async Task OrderIsRejected()
        {
            var order = await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.GetById(ScenarioHelper.Instance.OrderId);
            Assert.IsNotNull(order);
            Assert.IsTrue(order.State == State.REJECTED);
        }

        [Then(@"order is in filled state")]
        public async Task OrderIsInFilledState()
        {
            var order = await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.GetById(ScenarioHelper.Instance.OrderId); 
            Assert.IsNotNull(order);
            Assert.IsTrue(order.State == State.FILLED);
        }

        [Then(@"child order was sent")]
        public async Task ChildOrderWasSent()
        {
            var child = await ScenarioHelper.Instance.GetLastChild();
            Assert.IsTrue(child.ParentOrderId == ScenarioHelper.Instance.OrderId);
        }

        [Then(@"child order should not be sent")]
        public async Task ChildOrderShouldNotBeSent()
        {
            var childCount = (await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.GetAllChilds(ScenarioHelper.Instance.OrderId)).Count();
            Assert.True(childCount == ScenarioHelper.Instance.ChildOrders.Count());
        }

        [Then(@"child order is cancelled")]
        public async Task ChildOrderIsCancelled()
        {
            var child = await ScenarioHelper.Instance.GetLastChild();
            Assert.True(child.CancelState == State.PCANCEL);
            await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.Acknowledge(child.Id);
        }
    }
}