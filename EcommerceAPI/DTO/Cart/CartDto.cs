using EcommerceAPI.DTO.Products;

namespace EcommerceAPI.DTO.Cart
{
    public class CartDto
    {
        public int CartId { get; set; }
        public List<CartItemDto> Items { get; set; }
        public decimal Total { get; set; }
    }
}
