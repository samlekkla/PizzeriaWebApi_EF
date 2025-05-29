namespace PizzeriaWebApi_EF.DTO
{
    public class DishDto
    {
        public string DishName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int CategoryID { get; set; }
        public List<int> IngredientIDs { get; set; }
    }
}
