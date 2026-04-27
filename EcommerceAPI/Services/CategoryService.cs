using EcommerceAPI.DTO.Categories;
using EcommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> GetAllAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                }).ToListAsync();
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                throw new Exception("Category not found");

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }

        public async Task<Categories> CreateAsync(CreateCategoryDto dto)
        {
            var exists = await _context.Categories
                .AnyAsync(c => c.Name == dto.Name);

            if (exists)
                throw new Exception("Category already exists");

            var category = new Categories
            {
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task UpdateAsync(int id, UpdateCategoryDto dto)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                throw new Exception("Category not found");

            category.Name = dto.Name;
            category.Description = dto.Description;
            category.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
                throw new Exception("Category not found");

            // 🔥 Soft delete (mejor que borrar)
            category.IsActive = false;

            await _context.SaveChangesAsync();
        }
    }
}