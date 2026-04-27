using EcommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder()
        {
            var orderId = await _orderService.CreateOrderAsync();
            return Ok(new { orderId });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            return Ok(await _orderService.GetUserOrdersAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            return Ok(await _orderService.GetOrderByIdAsync(id));
        }
    }
}