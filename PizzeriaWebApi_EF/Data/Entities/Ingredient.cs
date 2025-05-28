using System.ComponentModel.DataAnnotations;

namespace PizzeriaWebApi_EF.Data.Entities
{
    public class Ingredient
    {
        [Key]
        public int IngredientID { get; set; }

        [Required, StringLength(51)]
        public string IngredientName { get; set; }

        public List<Dish> Dishes { get; set; } = new();
    }
}
