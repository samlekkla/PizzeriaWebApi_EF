using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PizzeriaWebApi_EF.Identity
{
    public class ApplicationUserContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationUserContext(DbContextOptions<ApplicationUserContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<ApplicationUser>("ApplicationUser")
                .HasValue<RegularUser>("RegularUser")
                .HasValue<AdminUser>("AdminUser");
        }


    }
}
