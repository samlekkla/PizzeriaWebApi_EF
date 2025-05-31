using PizzeriaWebApi_EF.Data.Interfaces;
using PizzeriaWebApi_EF.DTO;
using PizzeriaWebApi_EF.Identity;
using PizzeriaWebApi_EF.Middleware;

public class UserService : IUserService
{
    private readonly UserRepository _userRepository;
    private readonly JwtTokenGenerator _jwtTokenGenerator;

    public UserService(UserRepository userRepository, JwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

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
        return await _userRepository.CreateUserAsync(admin, dto.Password, "Admin");
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
        return await _userRepository.CreateUserAsync(user, dto.Password, "RegularUser");
    }

    public async Task<(string? token, string? role)> LoginWithRoleAsync(LoginDto dto)
    {
        ApplicationUser? user = null;

        if (!string.IsNullOrWhiteSpace(dto.Email))
        {
            user = await _userRepository.FindByEmailAsync(dto.Email);
        }

        if (user == null && !string.IsNullOrWhiteSpace(dto.UserName))
        {
            user = await _userRepository.FindByUsernameAsync(dto.UserName);
        }

        if (user == null || !await _userRepository.CheckPasswordAsync(user, dto.Password))
            return (null, null);

        var roles = await _userRepository.GetRolesAsync(user);
        var role = roles.FirstOrDefault();

        if (string.IsNullOrEmpty(role))
            return (null, null); // eller throw

        var token = _jwtTokenGenerator.GenerateToken(user, role);
        return (token, role);
    }

    public async Task<bool> UpdateAdminAsync(AdminUserDto dto, string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null || user.GetType().Name != nameof(AdminUser)) return false;

        user.UserName = dto.UserName;
        user.Email = dto.Email;
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.Phone;

        return await _userRepository.UpdateUserAsync(user);
    }

    public async Task<bool> UpdateUserAsync(ApplicationUser user)
    {
        return await _userRepository.UpdateUserAsync(user);
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
    {
        return await _userRepository.GetUserByIdAsync(userId);
    }

    public async Task<AdminUser?> GetAdminUserByIdAsync(string userId)
    {
        return await _userRepository.GetAdminUserByIdAsync(userId);
    }

    public async Task<RegularUser?> GetRegularUserByIdAsync(string userId)
    {
        return await _userRepository.GetRegularUserByIdAsync(userId);
    }

    public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
    {
        return await _userRepository.GetRolesAsync(user);
    }

    public async Task<bool> AnyAdminExistsAsync()
    {
        return await _userRepository.AnyAdminExistsAsync();
    }

    public async Task<bool> PromoteUserToPremiumAsync(string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) return false;

        var roles = await _userRepository.GetRolesAsync(user);
        if (!roles.Contains("PremiumUser"))
        {
            return await _userRepository.AddToRoleAsync(user, "PremiumUser");
        }

        return true;
    }

    public async Task<bool> DemoteUserToRegularAsync(string userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) return false;

        var roles = await _userRepository.GetRolesAsync(user);
        if (roles.Contains("PremiumUser"))
        {
            await _userRepository.RemoveFromRoleAsync(user, "PremiumUser");
            if (!roles.Contains("RegularUser"))
            {
                return await _userRepository.AddToRoleAsync(user, "RegularUser");
            }
        }

        return true;
    }
}
