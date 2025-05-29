namespace PizzeriaWebApi_EF.DTO
{
    public class OrderItemDto
    {
        public string DishName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }
}
