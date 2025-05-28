using System.ComponentModel.DataAnnotations;

namespace PizzeriaWebApi_EF.Data.Entities
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required, StringLength(51)]
        public string CategoryName { get; set; }
        public List<Dish> Dishes { get; set; } = new();
    }
}
