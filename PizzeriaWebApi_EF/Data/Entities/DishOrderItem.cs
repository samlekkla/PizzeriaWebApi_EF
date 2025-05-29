using System.ComponentModel.DataAnnotations;

namespace PizzeriaWebApi_EF.Data.Entities
{
    public class DishOrderItem
    {
        [Required]
        public int DishID { get; set; }

        [Range(1, 100)]
        public int Quantity { get; set; }
    }
}
