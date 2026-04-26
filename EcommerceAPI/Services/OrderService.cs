namespace EcommerceAPI.Services
{
    using EcommerceAPI.DTO.Orders;
    using EcommerceAPI.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Security.Claims;
    public class OrderService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateOrderAsync(CreateOrderDto dto)
        {
            // 1. Obtener usuario
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId");

            if (userIdClaim == null)
                throw new Exception("User not authenticated");

            int userId = int.Parse(userIdClaim.Value);

            // 2. Obtener carrito con items y productos
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Cart is empty");

            // 3. Crear orden
            var order = new Orders
            {
                UserId = userId,
                Status = "Pending",
                OrderDate = DateTime.Now,
                TotalAmount = 0
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync(); // necesario para tener Order.Id

            decimal total = 0;

            // 4. Crear OrderItems
            foreach (var item in cart.CartItems)
            {
                var orderItem = new OrderItems
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price // ⚠️ se congela el precio
                };

                total += item.Quantity * item.Product.Price;

                _context.OrderItems.Add(orderItem);
            }

            // 5. Actualizar total
            order.TotalAmount = total;

            // 6. Vaciar carrito
            _context.CartItems.RemoveRange(cart.CartItems);

            await _context.SaveChangesAsync();
        }
        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst("UserId");

            if (userIdClaim == null)
                throw new Exception("User not authenticated");

            int userId = int.Parse(userIdClaim.Value);

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            }).ToList();
        }
    }
}