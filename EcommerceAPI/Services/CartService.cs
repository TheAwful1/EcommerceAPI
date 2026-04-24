namespace EcommerceAPI.Services
{
    using EcommerceAPI.DTO.Cart;
    using EcommerceAPI.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;

    public class CartService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CartDto> GetCartAsync()
        {
            // 1. Obtener UserId del token
            var userIdClaim = _httpContextAccessor.HttpContext.User
                .FindFirst("UserId");

            if (userIdClaim == null)
                throw new Exception("User not authenticated");

            int userId = int.Parse(userIdClaim.Value);

            // 2. Buscar carrito con items + productos
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return new CartDto
                {
                    Items = new List<CartItemDto>(),
                    Total = 0
                };

            // 3. Mapear a DTO
            var items = cart.CartItems.Select(ci => new CartItemDto
            {
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                Price = ci.Product.Price,
                Quantity = ci.Quantity
            }).ToList();

            // 4. Calcular total
            var total = items.Sum(i => i.Price * i.Quantity);

            return new CartDto
            {
                Items = items,
                Total = total
            };
        }

        public async Task AddToCartAsync(AddToCartDto dto)
        {
            // 1. Obtener UserId desde el token
            var userIdClaim = _httpContextAccessor.HttpContext.User
                .FindFirst("UserId");

            if (userIdClaim == null)
                throw new Exception("User not authenticated");

            int userId = int.Parse(userIdClaim.Value);

            // 2. Validar producto existe
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == dto.ProductId);

            if (product == null)
                throw new Exception("Product not found");

            // 3. Buscar carrito del usuario
            var cart = await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // 4. Si no existe, crearlo
            if (cart == null)
            {
                cart = new Carts
                {
                    UserId = userId
                };

                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // 5. Buscar si el producto ya está en el carrito
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(ci =>
                    ci.CartId == cart.Id &&
                    ci.ProductId == dto.ProductId);

            if (cartItem != null)
            {
                // 6. Si existe → sumar cantidad
                cartItem.Quantity += dto.Quantity;
            }
            else
            {
                // 7. Si no existe → crear nuevo item
                cartItem = new CartItems
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                };

                _context.CartItems.Add(cartItem);
            }

            // 8. Guardar cambios
            await _context.SaveChangesAsync();
        }
    }
}
