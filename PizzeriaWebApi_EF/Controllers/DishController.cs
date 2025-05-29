using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebApi_EF.Data.Entities;
using PizzeriaWebApi_EF.Data.Interfaces;
using PizzeriaWebApi_EF.DTO;
using TomasosPizzeria_API.Data.Interfaces;

namespace TomasosPizzeria_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;
        private readonly ICategoryService _catService;
        private readonly IIngredientService _ingredientService;

        public DishController(IDishService dishService, ICategoryService catService, IIngredientService ingredientService)
        {
            _dishService = dishService;
            _catService = catService;
            _ingredientService = ingredientService;
        }

        [HttpPost]
        public async Task<IActionResult> AddDish([FromBody] DishDto dto)
        {
            var category = await _catService.GetCategoryByIdAsync(dto.CategoryID);
            if (category == null) return BadRequest("Ogiltig kategori");

            var ingredients = new List<Ingredient>();
            foreach (var id in dto.IngredientIDs)
            {
                var ing = await _ingredientService.GetIngredientByIdAsync(id);
                if (ing != null) ingredients.Add(ing);
            }

            var dish = new Dish
            {
                DishName = dto.DishName,
                Price = dto.Price,
                Description = dto.Description,
                Category = category,
                Ingredients = ingredients
            };

            await _dishService.AddDishAsync(dish);
            return Ok(dish);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDish(int id, [FromBody] DishDto dto)
        {
            var dish = await _dishService.GetDishByIdAsync(id);
            if (dish == null) return NotFound();

            var category = await _catService.GetCategoryByIdAsync(dto.CategoryID);
            if (category == null) return BadRequest("Ogiltig kategori");

            var ingredients = new List<Ingredient>();
            foreach (var ingredientId in dto.IngredientIDs)
            {
                var ing = await _ingredientService.GetIngredientByIdAsync(ingredientId);
                if (ing != null) ingredients.Add(ing);
            }

            dish.DishName = dto.DishName;
            dish.Price = dto.Price;
            dish.Description = dto.Description;
            dish.Category = category;
            dish.Ingredients = ingredients;

            await _dishService.UpdateDishAsync(dish);
            return Ok(dish);
        }
    }
}
