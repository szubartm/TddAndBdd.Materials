using Common;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem;
using TradingSystemWebApiContract;

namespace TradingSystemWebApi.Controllers
{
    [ApiController]
    [Route("order")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderManager _orderManager;

        public OrderController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        [Route("insert")]
        [HttpPost]
        public IActionResult Insert([FromBody]OrderDto orderDto)
        {
            var result = _orderManager.Insert(orderDto.Symbol, orderDto.Side, orderDto.Price, orderDto.Quantity, orderDto.TimeInForce, orderDto.ParentOrderId, orderDto.Strategy);
            return Ok(result);
        }

        [Route("acknowledge")]
        [HttpPost]
        public IActionResult Acknowledge([FromBody]string id)
        {
            var result = _orderManager.Acknowledge(id);
            return Ok(result);
        }

        [Route("cancel")]
        [HttpPost]
        public IActionResult Cancel([FromBody]string id)
        {
            var result = _orderManager.Cancel(id);
            return Ok(result);
        }

        [Route("fill")]
        [HttpPost]
        public IActionResult Fill([FromBody]OrderFillDto orderFillDto)
        {
            var result = _orderManager.Fill(orderFillDto.Id, orderFillDto.Quantity);
            return Ok(result);
        }

        [Route("reject")]
        [HttpPost]
        public IActionResult Reject([FromBody]string id)
        {
            var result = _orderManager.Reject(id);
            return Ok(result);
        }

        [Route("update")]
        [HttpPost]
        public IActionResult Update([FromBody]OrderUpdateDto orderUpdateDto)
        {
            var result = _orderManager.Update(orderUpdateDto.Id, orderUpdateDto.NewPrice, orderUpdateDto.NewQuantity);
            return Ok(result);
        }

        [Route("get")]
        [HttpGet]
        public IActionResult GetById(string id)
        {
            var result = _orderManager.GetById(id);
            return Ok(result);
        }

        [Route("getChilds")]
        [HttpGet]
        public IActionResult GetAllChilds(string id)
        {
            var result = _orderManager.GetAllChilds(id);
            return Ok(result);
        }

        [Route("GetAll")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _orderManager.GetAll();
            return Ok(result);
        }
    }
}
