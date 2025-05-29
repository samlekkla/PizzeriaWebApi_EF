namespace PizzeriaWebApi_EF.DTO
{
    public class OrderResponseDto
    {
        public int OrderID { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
