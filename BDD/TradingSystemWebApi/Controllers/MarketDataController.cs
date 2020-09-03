using DataProviderSystem;
using Microsoft.AspNetCore.Mvc;

namespace TradingSystemWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarketDataController : ControllerBase
    {
        private readonly IMarketDataSimulator _marketDataSimulator;

        public MarketDataController(IMarketDataSimulator marketDataSimulator)
        {
            _marketDataSimulator = marketDataSimulator;
        }

        [Route("insert")]
        [HttpPost]
        public IActionResult InsertNewMarketData([FromBody] TradingSystemWebApiContract.MarketDataDto marketDataDto)
        {
            _marketDataSimulator.InsertNewMarketData(marketDataDto.Symbol, marketDataDto.Bid, marketDataDto.Ask, marketDataDto.BidSize, marketDataDto.AskSize);
            return Ok();
        }

        [Route("clear")]
        [HttpPost]
        public IActionResult ClearMarketData()
        {
            _marketDataSimulator.ClearMarketData();
            return Ok();
        }
    }
}