using PizzeriaWebApi_EF.Data.Entities;
using PizzeriaWebApi_EF.Data.Interfaces;

namespace PizzeriaWebApi_EF.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IngredientRepository _repo;

        public IngredientService(IngredientRepository repo)
        {
            _repo = repo;
        }

        public async Task AddIngredientAsync(string name)
        {
            var ingredient = new Ingredient { IngredientName = name };
            await _repo.AddIngredientAsync(ingredient);
        }

        public async Task UpdateIngredientAsync(int id, string name)
        {
            var ingredient = await _repo.GetIngredientByIdAsync(id);
            if (ingredient == null) return;

            ingredient.IngredientName = name;
            await _repo.UpdateIngredientAsync(ingredient);
        }

        public async Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            return await _repo.GetAllIngredientsAsync();
        }

        public async Task<Ingredient?> GetIngredientByIdAsync(int id)
        {
            return await _repo.GetIngredientByIdAsync(id);
        }

        public async Task DeleteIngredientAsync(int id)
        {
            await _repo.DeleteIngredientAsync(id);
        }
    }
}
