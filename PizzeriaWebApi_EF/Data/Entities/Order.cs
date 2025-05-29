using System.ComponentModel.DataAnnotations;

namespace PizzeriaWebApi_EF.Data.Entities
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public string UserID { get; set; }

        [Required]
        public List<OrderItem> Items { get; set; } = new();

        [Range(0.01, 10000)]
        public decimal TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
