using PizzeriaWebApi_EF.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace PizzeriaWebApi_EF.DTO
{
    public class OrderDto
    {
        [Required]
        public List<DishOrderItem> Items { get; set; }
    }
}
