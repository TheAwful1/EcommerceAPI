using EcommerceAPI.DTO.Products;

namespace EcommerceAPI.DTO.Cart
{
    public class CartDto
    {
        public List<CartItemDto> Items { get; set; }
        public decimal Total { get; set; }
    }
}
