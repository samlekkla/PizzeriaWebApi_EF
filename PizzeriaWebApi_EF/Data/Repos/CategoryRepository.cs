using Microsoft.EntityFrameworkCore;
using PizzeriaWebApi_EF.Data;
using PizzeriaWebApi_EF.Data.Entities;

namespace TomasosPizzeria_API.Data.Repos
{
    public class CategoryRepository
    {
        private readonly ApplicationContext _context;

        public CategoryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.Include(c => c.Dishes).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await SaveAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await SaveAsync();
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await SaveAsync();
            }
        }

        private async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
