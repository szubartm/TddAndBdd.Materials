using System;
using Microsoft.AspNetCore.Mvc;
using TradingSystem;

namespace TradingSystemWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TimeController : ControllerBase
    {
        private readonly ITimeSimulator _timeSimulator;

        public TimeController(ITimeSimulator timeSimulator)
        {
            _timeSimulator = timeSimulator;
        }

        [Route("set")]
        [HttpPut]
        public IActionResult SetTime([FromBody]DateTime time)
        {
            _timeSimulator.SetTimeTo(time);
            return Ok();
        }

        [Route("update")]
        [HttpPut]
        public IActionResult UpdateTime([FromBody]TimeSpan time)
        {
            _timeSimulator.UpdateTimeBy(time);
            return Ok();
        }
    }
}
