using PizzeriaWebApi_EF.Data.Entities;
using TomasosPizzeria_API.Data.Interfaces;
using TomasosPizzeria_API.Data.Repos;

namespace TomasosPizzeria_API.Services
{
    public class CatagoryService : ICategoryService
    {
        private readonly CategoryRepository _repo;

        public CatagoryService(CategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task AddCategoryAsync(string name)
        {
            var category = new Category { CategoryName = name };
            await _repo.AddCategoryAsync(category);
        }

        public async Task UpdateCategoryAsync(int id, string name)
        {
            var category = await _repo.GetCategoryByIdAsync(id);
            if (category == null) return;

            category.CategoryName = name;
            await _repo.UpdateCategoryAsync(category);
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _repo.GetAllCategoriesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _repo.GetCategoryByIdAsync(id);
        }

        public async Task DeleteCategoryAsync(int id)
        {
            await _repo.DeleteCategoryAsync(id);
        }
    }
}
