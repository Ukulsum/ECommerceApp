using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.DTOs.CategoryDTOs;
using ECommerceApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Services
{
    public class CategoryService
    {
        private readonly ApplicationDbContext _context;
        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ApiResponse<CategoryResponseDTO>> CreateCategoryAsync(CategoryResponseDTO categoryDto)
        {
            try
            {
                // Check if category name already exists (case-insensitive)
                if(await _context.Categories.AnyAsync(c => c.Name == categoryDto.Name.ToLower()))
                {
                    return new ApiResponse<CategoryResponseDTO>(400, "Category name already exists.");
                }

                // Manual mapping from DTO to Model
                var category = new Category
                {
                    Name = categoryDto.Name,
                    Description = categoryDto.Description,
                    IsActive = true
                };

                // Add category to the database
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                // Map to CategoryResponseDTO
                var categoryResponse = new CategoryResponseDTO
                {
                    Id = category.Id,
                    Name = category.Name,
                    Description = category.Description,
                    IsActive = category.IsActive,
                };

                return new ApiResponse<CategoryResponseDTO>(200, categoryResponse);
            }
            catch (Exception ex)
            {
                // Log the exception (implementation depends on your logging setup)
                return new ApiResponse<CategoryResponseDTO>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
    }
}
