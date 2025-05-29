using Microsoft.EntityFrameworkCore;
using PizzeriaWebApi_EF.Data;
using PizzeriaWebApi_EF.Data.Entities;

namespace TomasosPizzeria_API.Data.Repos
{
    public class DishRepository
    {
        private readonly ApplicationContext _context;

        public DishRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<List<Dish>> GetAllDishesAsync()
        {
            return await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Ingredients)
                .ToListAsync();
        }

        public async Task<Dish> GetDishByIdAsync(int id)
        {
            return await _context.Dishes
                .Include(d => d.Category)
                .Include(d => d.Ingredients)
                .FirstOrDefaultAsync(d => d.DishID == id);
        }

        public async Task AddDishAsync(Dish dish)
        {
            _context.Dishes.Add(dish);
            await SaveAsync();
        }

        public async Task UpdateDishAsync(Dish dish)
        {
            _context.Dishes.Update(dish);
            await SaveAsync();
        }

        public async Task DeleteDishAsync(int id)
        {
            var dish = await _context.Dishes.FindAsync(id);
            if (dish != null)
            {
                _context.Dishes.Remove(dish);
                await SaveAsync();
            }
        }

        private async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
