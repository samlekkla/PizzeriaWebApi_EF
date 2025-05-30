using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebApi_EF.Data.Interfaces;

namespace PizzeriaWebApi_EF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public AdminUserController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("promote/{userId}")]
        public async Task<IActionResult> PromoteUserToPremium(string userId)
        {
            var success = await _orderService.PromoteUserToPremiumAsync(userId);
            if (!success) return NotFound("User not found");
            return Ok("User promoted to PremiumUser");
        }

        [HttpPost("demote/{userId}")]
        public async Task<IActionResult> DemoteUserToRegular(string userId)
        {
            var success = await _orderService.DemoteUserToRegularAsync(userId);
            if (!success) return NotFound("User not found or not Premium");
            return Ok("User demoted to RegularUser");
        }
    }

}
