using PizzeriaWebApi_EF.Data.Entities;
using PizzeriaWebApi_EF.DTO;

namespace PizzeriaWebApi_EF.Data.Interfaces
{
    public interface IOrderService
    {
        Task CreateOrderAsync(string userId, List<DishOrderItem> items);
        Task<UserOrdersSummaryDto> GetOrdersByUserIdAsync(string userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus);
    }
}
