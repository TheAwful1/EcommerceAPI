using EcommerceAPI.DTO.Cart;
using EcommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // 🔥 solo usuarios autenticados
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartDto dto)
        {
            await _cartService.AddToCartAsync(dto);
            return Ok(new { message = "Added to cart" });
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            return Ok(await _cartService.GetCartAsync());
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            await _cartService.RemoveItemAsync(productId);
            return Ok(new { message = "Item removed" });
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCartAsync();
            return Ok(new { message = "Cart cleared" });
        }
    }
}