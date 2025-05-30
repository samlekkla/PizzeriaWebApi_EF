using Microsoft.AspNetCore.Identity;
using PizzeriaWebApi_EF.Data.Entities;
using PizzeriaWebApi_EF.Data.Enums;
using PizzeriaWebApi_EF.Data.Interfaces;
using PizzeriaWebApi_EF.DTO;
using PizzeriaWebApi_EF.Identity;

public class OrderService : IOrderService
{
    private readonly OrderRepository _orderRepo;
    private readonly UserManager<ApplicationUser> _userManager;

    public OrderService(OrderRepository orderRepo, UserManager<ApplicationUser> userManager)
    {
        _orderRepo = orderRepo;
        _userManager = userManager;
    }

    public async Task CreateOrderAsync(string userId, List<DishOrderItem> items)
    {
        var dishIds = items.Select(i => i.DishID).ToList();
        var dishes = await _orderRepo.GetDishesByIdsAsync(dishIds);

        var orderItems = new List<OrderItem>();
        decimal totalPrice = 0;

        foreach (var item in items)
        {
            var dish = dishes.FirstOrDefault(d => d.DishID == item.DishID);
            if (dish != null)
            {
                totalPrice += dish.Price * item.Quantity;
                orderItems.Add(new OrderItem { DishID = dish.DishID, Dish = dish, Quantity = item.Quantity });
            }
        }

        var user = await _userManager.FindByIdAsync(userId);
        var roles = await _userManager.GetRolesAsync(user);
        bool isPremium = roles.Contains("PremiumUser");

        bool usedBonus = false;
        if (isPremium)
        {
            if (orderItems.Sum(i => i.Quantity) >= 3)
            {
                totalPrice *= 0.8m; // 20% discount
            }

            var bonusPoints = await _orderRepo.GetUserBonusPointsAsync(userId);
            if (bonusPoints >= 100)
            {
                var mostExpensive = orderItems.OrderByDescending(i => i.Dish.Price).FirstOrDefault();
                if (mostExpensive != null)
                {
                    totalPrice -= mostExpensive.Dish.Price;
                    usedBonus = true;
                }
            }
        }

        var order = new Order
        {
            UserID = userId,
            Items = orderItems,
            TotalPrice = totalPrice,
            UsedBonus = usedBonus,
            Status = OrderStatus.Pending
        };

        await _orderRepo.AddOrderAsync(order);
    }

    public async Task<UserOrdersSummaryDto> GetOrdersByUserIdAsync(string userId)
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
                DishName = i.Dish.DishName,
                Quantity = i.Quantity,
                UnitPrice = i.Dish.Price
            }).ToList()
        }).ToList();

        var totalSpent = orderDtos.Sum(o => o.TotalPrice);
        var bonusPoints = orderDtos.Sum(o => o.Items.Sum(i => i.Quantity * 10));

        return new UserOrdersSummaryDto
        {
            TotalSpent = totalSpent,
            BonusPoints = bonusPoints,
            Orders = orderDtos
        };
    }

    public async Task<bool> PromoteUserToPremiumAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var roles = await _userManager.GetRolesAsync(user);
        if (!roles.Contains("PremiumUser"))
        {
            await _userManager.AddToRoleAsync(user, "PremiumUser");
        }

        return true;
    }

    public async Task<bool> DemoteUserToRegularAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("PremiumUser"))
        {
            await _userManager.RemoveFromRoleAsync(user, "PremiumUser");
            if (!roles.Contains("RegularUser"))
            {
                await _userManager.AddToRoleAsync(user, "RegularUser");
            }
        }

        return true;
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
