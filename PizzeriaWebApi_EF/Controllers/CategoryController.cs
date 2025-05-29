using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PizzeriaWebApi_EF.DTO;
using TomasosPizzeria_API.Data.Interfaces;

namespace TomasosPizzeria_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto dto)
        {
            await _service.AddCategoryAsync(dto.CategoryName);
            return Ok("New category added!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto)
        {
            await _service.UpdateCategoryAsync(id, dto.CategoryName);
            return Ok("Category changed!");
        }
    }
}
