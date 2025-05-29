using PizzeriaWebApi_EF.Data.Entities;
using PizzeriaWebApi_EF.Data.Interfaces;
using PizzeriaWebApi_EF.DTO;

public class OrderService : IOrderService
{
    private readonly OrderRepository _orderRepo;

    public OrderService(OrderRepository orderRepo)
    {
        _orderRepo = orderRepo;
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

        var order = new Order
        {
            UserID = userId,
            Items = orderItems,
            TotalPrice = totalPrice
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
            Items = o.Items.Select(i => new OrderItemDto
            {
                DishName = i.Dish.DishName,
                Quantity = i.Quantity,
                UnitPrice = i.Dish.Price
            }).ToList()
        }).ToList();

        var totalSpent = orderDtos.Sum(o => o.TotalPrice);

        return new UserOrdersSummaryDto
        {
            TotalSpent = totalSpent,
            Orders = orderDtos
        };
    }
}
