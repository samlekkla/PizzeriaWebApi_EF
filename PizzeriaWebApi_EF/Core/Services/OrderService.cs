using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzeriaWebApi_EF.Data;
using PizzeriaWebApi_EF.Data.Entities;
using PizzeriaWebApi_EF.Data.Enums;
using PizzeriaWebApi_EF.Data.Interfaces;
using PizzeriaWebApi_EF.DTO;
using PizzeriaWebApi_EF.Identity;

public class OrderService : IOrderService
{
    private readonly OrderRepository _orderRepo;
    private readonly UserRepository _userRepository;
    private readonly ApplicationContext _context;

    public OrderService(OrderRepository orderRepo, UserRepository userRepository, ApplicationContext context)
    {
        _orderRepo = orderRepo;
        _userRepository = userRepository;
        _context = context;
    }

    public async Task CreateOrderAsync(string userId, List<DishOrderItem> itemsDto)
    {
        var user = await _userRepository.GetUserByIdAsync(userId)
            ?? throw new Exception("User not found");

        var roles = await _userRepository.GetRolesAsync(user);
        bool isPremium = roles.Contains("PremiumUser");

        bool usedBonus = false;
        decimal totalPrice = 0;

        var order = new Order
        {
            UserID = userId,
            CreatedAt = DateTime.UtcNow,
            Items = new List<OrderItem>(),
            Status = OrderStatus.Pending
        };

        foreach (var itemDto in itemsDto)
        {
            if (itemDto == null) continue;

            var dish = await _context.Dishes.FindAsync(itemDto.DishID);
            if (dish == null) continue;

            totalPrice += dish.Price * itemDto.Quantity;

            order.Items.Add(new OrderItem
            {
                DishID = itemDto.DishID,
                Quantity = itemDto.Quantity,
                Dish = dish,
                Order = order
            });
        }

        if (!order.Items.Any())
            throw new Exception("No valid dishes found for the order.");

        if (isPremium && order.Items.Sum(i => i.Quantity) >= 3)
        {
            totalPrice *= 0.8m;
        }

        if (isPremium)
        {
            var regUser = await _userRepository.GetRegularUserByIdAsync(userId);
            if (regUser != null)
            {
                regUser.BonusPoints += order.Items.Sum(i => i.Quantity) * 10;

                if (regUser.BonusPoints >= 100)
                {
                    totalPrice = 0;
                    regUser.BonusPoints -= 100;
                    usedBonus = true;
                }

                await _userRepository.UpdateUserAsync(regUser);
            }
        }

        order.TotalPrice = totalPrice;
        order.UsedBonus = usedBonus;

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
    }

    public async Task<UserOrdersSummaryDto?> GetOrdersByUserIdAsync(string userId)
    {
        var orders = await _orderRepo.GetOrdersByUserIdAsync(userId);

        var orderDtos = orders.Select(o => new OrderResponseDto
        {
            OrderID = o.OrderID,
            CreatedAt = o.CreatedAt,
            TotalPrice = o.TotalPrice,
            UsedBonus = o.UsedBonus,
            Status = o.Status.ToString(),
            Items = o.Items.Select(i => new OrderItemDto
            {
                DishName = i.Dish?.DishName ?? "Unknown",
                Quantity = i.Quantity,
                UnitPrice = i.Dish?.Price ?? 0
            }).ToList()
        }).ToList();

        var totalSpent = orderDtos.Sum(o => o.TotalPrice);

        var user = await _userRepository.GetUserByIdAsync(userId)
            ?? throw new Exception("User not found");

        var roles = await _userRepository.GetRolesAsync(user);
        int bonusPoints = 0;

        if (roles.Contains("PremiumUser"))
        {
            var regUser = await _userRepository.GetRegularUserByIdAsync(userId);
            if (regUser != null)
                bonusPoints = regUser.BonusPoints;
        }

        return new UserOrdersSummaryDto
        {
            TotalSpent = totalSpent,
            BonusPoints = bonusPoints,
            Orders = orderDtos
        };
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, string newStatus)
    {
        var order = await _orderRepo.GetOrderByIdAsync(orderId);
        if (order == null) return false;

        if (!Enum.TryParse<OrderStatus>(newStatus, true, out var parsedStatus))
            return false;

        order.Status = parsedStatus;
        await _orderRepo.UpdateOrderAsync(order);
        return true;
    }
}
