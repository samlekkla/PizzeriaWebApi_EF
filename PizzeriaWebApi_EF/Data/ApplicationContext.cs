using Microsoft.EntityFrameworkCore;
using PizzeriaWebApi_EF.Data.Entities;

namespace PizzeriaWebApi_EF.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public virtual DbSet<Dish> Dishes { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }

    }


}
