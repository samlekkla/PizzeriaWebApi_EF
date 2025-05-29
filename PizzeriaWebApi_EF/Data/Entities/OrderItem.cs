using System.ComponentModel.DataAnnotations;

namespace PizzeriaWebApi_EF.Data.Entities
{
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }

        [Required]
        public int DishID { get; set; }

        [Required]
        public Dish Dish { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        public int OrderID { get; set; }

        [Required]
        public Order Order { get; set; }
    }
}
