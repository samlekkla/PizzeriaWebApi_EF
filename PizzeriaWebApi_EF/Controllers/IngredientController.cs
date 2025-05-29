using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebApi_EF.DTO;
using PizzeriaWebApi_EF.Data.Interfaces;

namespace TomasosPizzeria_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _service;

        public IngredientController(IIngredientService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddIngredient([FromBody] IngredientDto dto)
        {
            await _service.AddIngredientAsync(dto.IngredientName);
            return Ok("Ingredient added!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, [FromBody] IngredientDto dto)
        {
            await _service.UpdateIngredientAsync(id, dto.IngredientName);
            return Ok("Ingredient changed");
        }
    }
}
