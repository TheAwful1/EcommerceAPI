using EcommerceAPI.DTO.Orders;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _service;

        public OrdersController(OrderService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetOrder()
        {
            var orders = await _service.GetOrdersAsync();
            return Ok(orders);
        }
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CreateOrderDto dto)
        {
            await _service.CreateOrderAsync(dto);
            return Ok("Order created");
        }
    }
}
