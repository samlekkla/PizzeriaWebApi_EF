using PizzeriaWebApi_EF.DTO;
using PizzeriaWebApi_EF.Identity;

namespace PizzeriaWebApi_EF.Data.Interfaces
{
    public interface IUserService
    {
        Task<(string? token, string? role)> LoginWithRoleAsync(LoginDto dto);
        Task<ApplicationUser?> GetUserByIdAsync(string userId);
        Task<RegularUser?> GetRegularUserByIdAsync(string userId);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<bool> RegisterUserAsync(RegularUserDto dto);
        Task<bool> RegisterAdminAsync(AdminUserDto dto);
        Task<bool> UpdateAdminAsync(AdminUserDto dto, string userId);
        Task<bool> AnyAdminExistsAsync();
        Task<IList<string>> GetRolesAsync(ApplicationUser user);
        Task<AdminUser?> GetAdminUserByIdAsync(string userId);
        Task<bool> PromoteUserToPremiumAsync(string userId);
        Task<bool> DemoteUserToRegularAsync(string userId);
    }
}
