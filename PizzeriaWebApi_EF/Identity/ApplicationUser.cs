using Microsoft.AspNetCore.Identity;

namespace PizzeriaWebApi_EF.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
