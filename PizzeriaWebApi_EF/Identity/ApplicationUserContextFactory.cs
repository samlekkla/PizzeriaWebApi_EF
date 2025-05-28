using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace PizzeriaWebApi_EF.Identity
{
    public class ApplicationUserContextFactory : IDesignTimeDbContextFactory<ApplicationUserContext>
    {
        public ApplicationUserContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationUserContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new ApplicationUserContext(optionsBuilder.Options);
        }
    }

}
