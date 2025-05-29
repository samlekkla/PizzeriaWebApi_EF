namespace PizzeriaWebApi_EF.DTO
{
    public class UserOrdersSummaryDto
    {
        public decimal TotalSpent { get; set; }
        public List<OrderResponseDto> Orders { get; set; }
    }
}
