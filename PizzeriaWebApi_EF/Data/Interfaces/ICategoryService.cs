using PizzeriaWebApi_EF.Data.Entities;

namespace TomasosPizzeria_API.Data.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task AddCategoryAsync(string name);
        Task UpdateCategoryAsync(int id, string name);
        Task DeleteCategoryAsync(int id);
    }
}
