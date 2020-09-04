using System.Threading.Tasks;
using Behavioral.Tools.ScenarioHelpers;
using TechTalk.SpecFlow;

namespace Behavioral.Steps
{
    [Binding]
    public class OrderRelatedSteps
    {
        [Given("Store child orders")]
        [When("Store child orders")]
        [Then("Store child orders")]
        public Task StoreCurrentChildList() =>
            ScenarioHelper.Instance.StoreCurrentChildList();
    }
}