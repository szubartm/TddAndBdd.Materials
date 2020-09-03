using System.Threading.Tasks;
using Behavioral.Tools.ScenarioHelpers;
using Common;
using TechTalk.SpecFlow;

namespace Behavioral.Steps
{
    [Binding]
    public class OrderManipulationSteps
    {
        [When(@"child order is filled with (\d+) shares")]
        [Then(@"child order is filled with (\d+) shares")]
        public async Task ChildOrderIsFilledWithShares(int quantity)
        {
            var child = await ScenarioHelper.Instance.GetLastChild();
            if (child.State == State.UNACK)
                await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.Acknowledge(child.Id);
            await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.Fill(child.Id, quantity);
        }


        [When(@"child order is fully filled")]
        [Then(@"child order is fully filled")]
        public async Task ChildOrderIsFullyFilled()
        {
            var child = await ScenarioHelper.Instance.GetLastChild();
            await ChildOrderIsFilledWithShares(child.PendingQuantity);
        }

        [When(@"child order is rejected")]
        public async Task ChildOrderIsRejected()
        {
            var child = await ScenarioHelper.Instance.GetLastChild();
            await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.Reject(child.Id);
        }

        [When(@"child order is acknowledged")]
        public async Task ChildOrderIsAcknowledged()
        {
            var child = await ScenarioHelper.Instance.GetLastChild();
            await Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.Acknowledge(child.Id);
        }

      

        [When(@"order is updated to (\d+) price and (\d+) quantity")]
        public Task OrderIsUpdated(decimal price, int quantity) =>
             Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.Update(ScenarioHelper.Instance.OrderId, price, quantity);
        

        [When(@"order is cancelled")]
        public Task OrderIsCancelled()=>
            Hooks.Hooks.SimulationContext.OrderManagementSystemSimulator.Cancel(ScenarioHelper.Instance.OrderId);

    }
}