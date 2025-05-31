using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzeriaWebApi_EF.Data;
using PizzeriaWebApi_EF.Data.Enums;

namespace PizzeriaWebApi_EF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public AdminOrderController(ApplicationContext context)
        {
            _context = context;
        }


        [HttpPut("order-status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound("Order not found");

            if (!Enum.TryParse<OrderStatus>(status, true, out var newStatus))
            {
                return BadRequest("Invalid status value");
            }

            order.Status = newStatus;

            await _context.SaveChangesAsync();
            return Ok($"Order status updated to {order.Status}");
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Dish)
                .FirstOrDefaultAsync(o => o.OrderID == orderId);

            if (order == null) return NotFound("Order not found");

            var response = new
            {
                order.OrderID,
                order.UserID,
                order.CreatedAt,
                order.TotalPrice,
                order.Status,
                order.UsedBonus,
                Items = order.Items.Select(i => new
                {
                    i.DishID,
                    i.Dish.DishName,
                    i.Quantity,
                    UnitPrice = i.Dish.Price,
                    Total = i.Dish.Price * i.Quantity
                })
            };

            return Ok(response);
        }


        [HttpDelete("order/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return Ok("Order deleted");
        }


    }
}
