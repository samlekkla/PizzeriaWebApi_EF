using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebApi_EF.Data.Interfaces;
using PizzeriaWebApi_EF.DTO;
using System.Security.Claims;


[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{
    private readonly IUserService _userRepository;

    public AdminController(IUserService userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("create-admin")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAdmin([FromBody] AdminUserDto dto)
    {
        if (await _userRepository.AnyAdminExistsAsync() &&
         !(User.Identity?.IsAuthenticated == true && User.IsInRole("Admin")))
        {
            return Unauthorized("Endast en administratör får skapa nya admin-användare.");
        }

        var result = await _userRepository.RegisterAdminAsync(dto);
        if (!result) return BadRequest("Kunde inte skapa adminanvändare");
        return Ok("Adminanvändare skapad");
    }

    [HttpGet("me")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetMyAdminInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token");

        var adminUser = await _userRepository.GetAdminUserByIdAsync(userId);

        if (adminUser == null)
            return NotFound("Admin user not found");

        var roles = await _userRepository.GetRolesAsync(adminUser);

        return Ok(new
        {
            adminUser.UserName,
            adminUser.Email,
            adminUser.PhoneNumber,
            adminUser.FirstName,
            adminUser.LastName,
            Roles = roles
        });
    }


    [HttpPut("update-admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateAdmin([FromBody] AdminUserDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // ⬇ Använd rätt metod nu
        var adminUser = await _userRepository.GetAdminUserByIdAsync(userId);

        if (adminUser == null)
            return BadRequest("Adminanvändare hittades inte eller har fel typ");

        adminUser.UserName = dto.UserName;
        adminUser.Email = dto.Email;
        adminUser.FirstName = dto.FirstName;
        adminUser.LastName = dto.LastName;
        adminUser.PhoneNumber = dto.Phone;

        var result = await _userRepository.UpdateUserAsync(adminUser);
        if (!result) return BadRequest("Uppdatering misslyckades");

        var roles = await _userRepository.GetRolesAsync(adminUser);

        return Ok(new
        {
            adminUser.UserName,
            adminUser.Email,
            adminUser.PhoneNumber,
            adminUser.FirstName,
            adminUser.LastName,
            Roles = roles
        });
    }
}