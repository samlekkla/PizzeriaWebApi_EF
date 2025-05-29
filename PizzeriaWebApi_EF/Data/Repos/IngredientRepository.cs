using Microsoft.EntityFrameworkCore;
using PizzeriaWebApi_EF.Data;
using PizzeriaWebApi_EF.Data.Entities;

namespace PizzeriaWebApi_EF.Services
{
    public class IngredientRepository
    {
        private readonly ApplicationContext _context;

        public IngredientRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task AddIngredientAsync(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await SaveAsync();
        }

        public async Task UpdateIngredientAsync(Ingredient ingredient)
        {
            _context.Ingredients.Update(ingredient);
            await SaveAsync();
        }

        public async Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            return await _context.Ingredients.Include(i => i.Dishes).ToListAsync();
        }

        public async Task<Ingredient> GetIngredientByIdAsync(int id)
        {
            return await _context.Ingredients.FindAsync(id);
        }

        public async Task DeleteIngredientAsync(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient != null)
            {
                _context.Ingredients.Remove(ingredient);
                await SaveAsync();
            }
        }

        private async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
