using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebApi_EF.Data.Interfaces;

namespace PizzeriaWebApi_EF.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminUserController : ControllerBase
    {
        private readonly IUserService _userService;

        public AdminUserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("promote/{userId}")]
        public async Task<IActionResult> PromoteUserToPremium(string userId)
        {
            var success = await _userService.PromoteUserToPremiumAsync(userId);
            if (!success) return NotFound("User not found");
            return Ok("User promoted to PremiumUser");
        }

        [HttpPost("demote/{userId}")]
        public async Task<IActionResult> DemoteUserToRegular(string userId)
        {
            var success = await _userService.DemoteUserToRegularAsync(userId);
            if (!success) return NotFound("User not found or not Premium");
            return Ok("User demoted to RegularUser");
        }
    }
}
