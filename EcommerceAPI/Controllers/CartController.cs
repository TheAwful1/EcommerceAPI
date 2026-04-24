using EcommerceAPI.DTO.Cart;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly CartService _service;

        public CartController(CartService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var cart = await _service.GetCartAsync();
            return Ok(cart);
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            await _service.AddToCartAsync(dto);
            return Ok("Added to cart");
        }
    }
}
