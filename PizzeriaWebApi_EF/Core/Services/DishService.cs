using PizzeriaWebApi_EF.Data.Entities;
using TomasosPizzeria_API.Data.Interfaces;
using TomasosPizzeria_API.Data.Repos;

namespace TomasosPizzeria_API.Services
{
    public class DishService : IDishService
    {
        private readonly DishRepository _repo;

        public DishService(DishRepository repo)
        {
            _repo = repo;
        }

        public async Task AddDishAsync(Dish dish)
        {
            await _repo.AddDishAsync(dish);
        }

        public async Task UpdateDishAsync(Dish dish)
        {
            await _repo.UpdateDishAsync(dish);
        }

        public async Task DeleteDishAsync(int id)
        {
            await _repo.DeleteDishAsync(id);
        }

        public async Task<Dish?> GetDishByIdAsync(int id)
        {
            return await _repo.GetDishByIdAsync(id);
        }

        public async Task<List<Dish>> GetAllDishesAsync()
        {
            return await _repo.GetAllDishesAsync();
        }
    }
}
