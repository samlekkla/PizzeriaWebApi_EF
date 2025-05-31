namespace PizzeriaWebApi_EF.DTO
{
    public class DishDto
    {
        public required string DishName { get; set; }
        public decimal Price { get; set; }
        public required string Description { get; set; }
        public int CategoryID { get; set; }
        public required List<int> IngredientIDs { get; set; }
    }
}
