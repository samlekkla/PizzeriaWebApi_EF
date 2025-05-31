// RegularUserController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebApi_EF.Data.Interfaces;
using PizzeriaWebApi_EF.DTO;
using System.Security.Claims;


[ApiController]
[Route("api/[controller]")]
public class RegularUserController : ControllerBase
{
    private readonly IUserService _userRepository;

    public RegularUserController(IUserService userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegularUserDto dto)
    {
        var result = await _userRepository.RegisterUserAsync(dto);
        if (!result) return BadRequest("Registration failed");

        return Ok("User registered");
    }

    [HttpGet("me")]
    [Authorize(Roles = "RegularUser")]
    public async Task<IActionResult> GetMyInfo()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token");

        // Hämta RegularUser för att få tillgång till alla egenskaper
        var regularUser = await _userRepository.GetRegularUserByIdAsync(userId);

        if (regularUser == null)
            return NotFound("Regular user not found");

        var roles = await _userRepository.GetRolesAsync(regularUser);

        return Ok(new
        {
            regularUser.UserName,
            regularUser.Email,
            regularUser.PhoneNumber,
            regularUser.FirstName,
            regularUser.LastName,
            Roles = roles
        });
    }

    [HttpPut("update")]
    [Authorize(Roles = "RegularUser")]
    public async Task<IActionResult> UpdateUser([FromBody] RegularUserDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
            return Unauthorized("User ID not found in token");

        var regularUser = await _userRepository.GetRegularUserByIdAsync(userId);

        if (regularUser == null)
            return BadRequest("User could not be found or wrong type");

        regularUser.UserName = dto.UserName;
        regularUser.Email = dto.Email;
        regularUser.FirstName = dto.FirstName;
        regularUser.LastName = dto.LastName;
        regularUser.PhoneNumber = dto.Phone;

        var result = await _userRepository.UpdateUserAsync(regularUser);
        if (!result) return BadRequest("Update failed");

        return Ok("Profile updated!");
    }


}