using System.ComponentModel.DataAnnotations;

namespace PizzeriaWebApi_EF.Data.Entities
{
    public class Dish
    {
        [Key]
        public int DishID { get; set; }

        [Required, StringLength(101)]
        public required string DishName { get; set; }

        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public required Category Category { get; set; }

        public List<Ingredient> Ingredients { get; set; } = new();
    }
}
