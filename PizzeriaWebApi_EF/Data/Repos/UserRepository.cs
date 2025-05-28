using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzeriaWebApi_EF.Data.Interfaces;
using PizzeriaWebApi_EF.DTO;
using PizzeriaWebApi_EF.Identity;
using PizzeriaWebApi_EF.Middleware;

public class UserRepository : IUserService
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public async Task<bool> RegisterAdminAsync(AdminUserDto dto)
    {
        var admin = new AdminUser
        {
            UserName = dto.UserName,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.Phone
        };

        var result = await _userManager.CreateAsync(admin, dto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(admin, "Admin");
        }

        return result.Succeeded;
    }

    public async Task<bool> UpdateAdminAsync(AdminUserDto dto, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.GetType().Name != nameof(AdminUser)) return false;

        user.UserName = dto.UserName;
        user.Email = dto.Email;
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.Phone;

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string userId)
    {
        return await _userManager.Users
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<AdminUser> GetAdminUserByIdAsync(string userId)
    {
        return await _userManager.Users
            .Where(u => u.Id == userId && EF.Property<string>(u, "Discriminator") == "AdminUser")
            .Cast<AdminUser>()
            .FirstOrDefaultAsync();
    }

    public async Task<RegularUser> GetRegularUserByIdAsync(string userId)
    {
        return await _userManager.Users
            .Where(u => u.Id == userId && EF.Property<string>(u, "Discriminator") == "RegularUser")
            .Cast<RegularUser>()
            .FirstOrDefaultAsync();
    }
    public async Task<bool> AnyAdminExistsAsync()
    {
        var allUsers = await _userManager.Users.ToListAsync();
        return allUsers.Any(u => u is AdminUser);
    }

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<bool> RegisterUserAsync(RegularUserDto dto)
    {
        var user = new RegularUser
        {
            UserName = dto.UserName,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            PhoneNumber = dto.Phone
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "RegularUser");
        }

        return result.Succeeded;
    }


    public UserRepository(UserManager<ApplicationUser> userManager, JwtTokenGenerator jwtTokenGenerator)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<bool> UpdateUserAsync(ApplicationUser user)
    {
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<(string token, string role)> LoginWithRoleAsync(LoginDto dto)
    {
        ApplicationUser user = null;

        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        }

        if (user == null && !string.IsNullOrWhiteSpace(dto.UserName))
        {
            user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == dto.UserName);
        }

        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return (null, null);

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault(); // Antar att användaren bara har en roll

        var token = _jwtTokenGenerator.GenerateToken(user, role);
        return (token, role);
    }
}

