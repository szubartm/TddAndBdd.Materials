using System.Threading.Tasks;
using Behavioral.Tools.ScenarioHelpers;
using TechTalk.SpecFlow;

namespace Behavioral.Steps
{
    [Binding]
    public class OrderRelatedSteps
    {
        private readonly ScenarioHelper _scenarioHelper;

        public OrderRelatedSteps(ScenarioHelper scenarioHelper)
        {
            _scenarioHelper = scenarioHelper;
        }
        [Given("Store child orders")]
        [When("Store child orders")]
        [Then("Store child orders")]
        public Task StoreCurrentChildList() =>
            _scenarioHelper.StoreCurrentChildList();
    }
}