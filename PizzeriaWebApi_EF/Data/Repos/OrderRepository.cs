using PizzeriaWebApi_EF.Data.Entities;
using PizzeriaWebApi_EF.Data;
using Microsoft.EntityFrameworkCore;

public class OrderRepository
{
    private readonly ApplicationContext _context;

    public OrderRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<List<Dish>> GetDishesByIdsAsync(List<int> dishIds)
    {
        return await _context.Dishes.Where(d => dishIds.Contains(d.DishID)).ToListAsync();
    }

    public async Task AddOrderAsync(Order order)
    {
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Dish)
            .Where(o => o.UserID == userId)
            .ToListAsync();
    }
}
