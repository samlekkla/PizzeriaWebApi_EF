using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PizzeriaWebApi_EF.DTO;
using PizzeriaWebApi_EF.Identity;

public class UserRepository
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserRepository(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<bool> CreateUserAsync(ApplicationUser user, string password, string role)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, role);
        }
        return result.Succeeded;
    }

    public async Task<bool> UpdateUserAsync(ApplicationUser user)
    {
        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<AdminUser?> GetAdminUserByIdAsync(string userId)
    {
        return await _userManager.Users
            .Where(u => u.Id == userId && EF.Property<string>(u, "Discriminator") == "AdminUser")
            .Cast<AdminUser>()
            .FirstOrDefaultAsync();
    }

    public async Task<RegularUser?> GetRegularUserByIdAsync(string userId)
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

    public async Task<bool> AddToRoleAsync(ApplicationUser user, string role)
    {
        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded;
    }

    public async Task<bool> RemoveFromRoleAsync(ApplicationUser user, string role)
    {
        var result = await _userManager.RemoveFromRoleAsync(user, role);
        return result.Succeeded;
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<ApplicationUser?> FindByUsernameAsync(string username)
    {
        return await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == username);
    }
}
