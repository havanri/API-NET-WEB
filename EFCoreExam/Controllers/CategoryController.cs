using EFCoreExam.DTOs.Category;
using EFCoreExam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace EFCoreExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMemoryCache _cache;

        public CategoryController(ICategoryService categoryService, IMemoryCache memoryCache)
        {
            _categoryService = categoryService;
            _cache = memoryCache;
        }

        [HttpGet]
        [Authorize(Policy = "RequireManagerRole")]
/*        [ResponseCache(Duration = 60)]*/
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
           
            if (_cache.TryGetValue("categories_list", out IEnumerable<CategoryDto> cachedData))
            {
                return Ok(cachedData);
            }
            var categories = await _categoryService.GetCategoriesAsync();

            // Add the new data to cache
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSize(1)
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(600));//.SetSlidingExpiration()
            _cache.Set("categories_list", categories, cacheEntryOptions);
            return Ok(categories);
        }
        private async Task<string> GetDataFromDataSourceAsync()
        {
            // Code to retrieve data from the data source
            await Task.Delay(1000);
            return "Some data from the data source";
        }
        [HttpGet("{id}")]
        /*[ResponseCache(Duration = 60)]*/
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            if (_cache.TryGetValue("category", out CategoryDto cachedData))
            {
                return Ok(cachedData);
            }
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSize(1)
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(600));//.SetSlidingExpiration()
            _cache.Set("category", category, cacheEntryOptions);
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromForm] CreateCategoryDto categoryDto)
        {
            await _categoryService.CreateCategoryAsync(categoryDto);
            return Ok(true);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromForm] UpdateCategoryDto updateCategoryDto)
        {
            if (id != updateCategoryDto.Id)
            {
                return BadRequest();
            }
            await _categoryService.UpdateCategoryAsync(id, updateCategoryDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return NoContent();
        }
    }
}
