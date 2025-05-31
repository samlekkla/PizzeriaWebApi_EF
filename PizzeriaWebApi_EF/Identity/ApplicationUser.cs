using Microsoft.AspNetCore.Identity;

namespace PizzeriaWebApi_EF.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
    }
}
