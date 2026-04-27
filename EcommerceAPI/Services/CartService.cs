using EcommerceAPI.DTO.Cart;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EcommerceAPI.Services
{
    public class CartService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User
                .FindFirst("UserId")?.Value;

            return int.Parse(userId);
        }

        public async Task AddToCartAsync(AddToCartDto dto)
        {
            var userId = GetUserId();

            var product = await _context.Products.FindAsync(dto.ProductId);
            if (product == null)
                throw new Exception("Product not found");

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Carts
                {
                    UserId = userId,
                    CartItems = new List<CartItems>()
                };

                _context.Carts.Add(cart);
            }

            var existingItem = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == dto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                cart.CartItems.Add(new CartItems
                {
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }

            await _context.SaveChangesAsync();
        }

        public async Task<CartDto> GetCartAsync()
        {
            var userId = GetUserId();

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return new CartDto { CartId = 0, Items = new List<CartItemDto>() };

            var items = cart.CartItems.Select(ci => new CartItemDto
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                Price = ci.Product.Price,
                Quantity = ci.Quantity
            }).ToList();

            return new CartDto
            {
                CartId = cart.Id,
                Items = items
            };
        }

        public async Task RemoveItemAsync(int productId)
        {
            var userId = GetUserId();

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                throw new Exception("Cart not found");

            var item = cart.CartItems
                .FirstOrDefault(ci => ci.ProductId == productId);

            if (item == null)
                throw new Exception("Item not found");

            cart.CartItems.Remove(item);

            await _context.SaveChangesAsync();
        }

        public async Task ClearCartAsync()
        {
            var userId = GetUserId();

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return;

            cart.CartItems.Clear();

            await _context.SaveChangesAsync();
        }
    }
}