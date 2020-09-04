using System;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Behavioral.Steps
{
    [Binding]
    public class TimeSteps
    {
        [Given(@"it is (.*)")]
        [When(@"it is (.*)")]
        [Then(@"it is (.*)")]
        public async Task ItIs(string hour)
        {
            var time = TimeSpan.Parse(hour);
            var today = DateTime.Today;
            today = today + time;
            await Hooks.Hooks.SimulationContext.TradingSystemClient.SetTimeTo(today);
        }
    }
}