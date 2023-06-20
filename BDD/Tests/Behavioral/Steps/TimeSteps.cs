using System;
using System.Linq;
using System.Threading.Tasks;
using Common;
using TechTalk.SpecFlow;
using TradingSystem;

namespace Behavioral.Steps
{
    [Binding]
    public class TimeSteps
    {
        // [Given(@"it is (.*)")]
        // [When(@"it is (.*)")]
        // [Then(@"it is (.*)")]
        // [Given(@"że jest (.*)")]
        // [When(@"jest (.*)")]
        // [Then(@"jest (.*)")]
        // public async Task ItIs(string hour)
        // {
        //     var time = TimeSpan.Parse(hour);
        //     var today = DateTime.Today;
        //     today = today + time;
        //     await Hooks.Hooks.SimulationContext.TradingSystemClient.SetTimeTo(today);
        // }
        [Given(@"it is (.*)/(.*)")]      
        public async Task ItIs(TradingSessionType session, TimeType type)
        {
           var today = DateTime.Today;
           var settings = Settings.TradingSessions.Where(x => x.SessionType == session);
           
           foreach(var setting in settings){
             if(type == TimeType.Beginning){
                today = today + setting.StartTime;
             }else if(type == TimeType.Middle)
             {
                today = today + (setting.EndTime - setting.StartTime)/2;
             }else{
                today = today + (setting.EndTime);
             }
           }

           await Hooks.Hooks.SimulationContext.TradingSystemClient.SetTimeTo(today);
        }
 



 


    }
}