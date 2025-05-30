using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebApi_EF.Data;

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

        [HttpDelete("order/{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return Ok("Order deleted");
        }

        [HttpPut("order-status/{orderId}")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] string status)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            // Om du har en statuskolumn, exempel:
            // order.Status = status;

            await _context.SaveChangesAsync();
            return Ok("Order status updated");
        }


    }
}
