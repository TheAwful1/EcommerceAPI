using EcommerceAPI.DTO;
using EcommerceAPI.DTO.Products;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            return await _context.Products
                .Include(p => p.Category)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryName = p.Category.Name,
                    ImageUrl = p.ProductImages.Select(i => i.ImageUrl).FirstOrDefault()
                }).ToListAsync();
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                throw new Exception("Product not found");

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                CategoryName = product.Category.Name,
                ImageUrl = product.ProductImages.Select(i => i.ImageUrl).FirstOrDefault()
    
            };
        }

        public async Task<Products> CreateAsync(CreateProductDto dto)
        {
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId);

            if (!categoryExists)
                throw new Exception("Category does not exist");

            var product = new Products
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
// ✅ Guardar imagen si viene
    if (!string.IsNullOrEmpty(dto.ImageUrl))
    {
        _context.ProductImages.Add(new ProductImages
        {
            ProductId = product.Id,
            ImageUrl = dto.ImageUrl,
            IsMain = true
        });
        await _context.SaveChangesAsync();
    }
            return product;
        }

        public async Task UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new Exception("Product not found");

            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Id == dto.CategoryId);

            if (!categoryExists)
                throw new Exception("Category does not exist");

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new Exception("Product not found");
            // ✅ Eliminar dependencias primero
            _context.ProductImages.RemoveRange(product.ProductImages);
            _context.CartItems.RemoveRange(product.CartItems);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}