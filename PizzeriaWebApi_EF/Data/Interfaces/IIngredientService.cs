using PizzeriaWebApi_EF.Data.Entities;

namespace PizzeriaWebApi_EF.Data.Interfaces
{
    public interface IIngredientService
    {
        Task<List<Ingredient>> GetAllIngredientsAsync();
        Task<Ingredient> GetIngredientByIdAsync(int id);
        Task AddIngredientAsync(string name);
        Task UpdateIngredientAsync(int id, string name);
        Task DeleteIngredientAsync(int id);
    }
}
