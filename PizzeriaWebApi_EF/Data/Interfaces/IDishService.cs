using PizzeriaWebApi_EF.Data.Entities;

namespace TomasosPizzeria_API.Data.Interfaces
{
    public interface IDishService
    {
        Task<List<Dish>> GetAllDishesAsync();
        Task<Dish?> GetDishByIdAsync(int id);
        Task AddDishAsync(Dish dish);
        Task UpdateDishAsync(Dish dish);
        Task DeleteDishAsync(int id);
    }
}
