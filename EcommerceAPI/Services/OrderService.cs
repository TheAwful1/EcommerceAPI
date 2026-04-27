using EcommerceAPI.DTO.Orders;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
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

        // 🔥 Crear orden desde carrito
        public async Task<int> CreateOrderAsync()
        {
            var userId = GetUserId();

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                throw new Exception("Cart is empty");

            var order = new Orders
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = "Pending",
                OrderItems = new List<OrderItems>()
            };

            decimal total = 0;

            foreach (var item in cart.CartItems)
            {
                var orderItem = new OrderItems
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price // 🔥 snapshot
                };

                total += orderItem.UnitPrice * orderItem.Quantity;

                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = total;

            _context.Orders.Add(order);

            // 🔥 limpiar carrito
            cart.CartItems.Clear();

            await _context.SaveChangesAsync();

            return order.Id;
        }

        // 🔥 Obtener órdenes del usuario
        public async Task<List<OrderDto>> GetUserOrdersAsync()
        {
            var userId = GetUserId();

            return await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    Items = o.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductName = oi.Product.Name,
                        UnitPrice = oi.UnitPrice,
                        Quantity = oi.Quantity
                    }).ToList()
                }).ToListAsync();
        }

        // 🔥 Obtener detalle de una orden
        public async Task<OrderDto> GetOrderByIdAsync(int orderId)
        {
            var userId = GetUserId();

            var order = await _context.Orders
                .Where(o => o.Id == orderId && o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync();

            if (order == null)
                throw new Exception("Order not found");

            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductName = oi.Product.Name,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList()
            };
        }
    }
}