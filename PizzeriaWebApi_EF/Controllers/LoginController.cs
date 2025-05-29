using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebApi_EF.Data.Interfaces;
using PizzeriaWebApi_EF.DTO;


[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IUserService _userRepository;

    public LoginController(IUserService userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var (token, role) = await _userRepository.LoginWithRoleAsync(dto);
        if (token == null) return Unauthorized("Wrong account info");
        return Ok(new { token, role });
    }
}
