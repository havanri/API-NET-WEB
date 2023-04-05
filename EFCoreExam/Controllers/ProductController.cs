using EFCoreExam.Models;
using EFCoreExam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.getAll();
            if (products == null)
            {
                return NotFound();
            }
            return Ok(products);
        }
        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Add([FromForm] CreateProductDto createProductDto)
        {
            if (string.IsNullOrEmpty(createProductDto.Name))
            {
                return BadRequest("Product name cannot be empty");
            }

            // Check if product price is negative
            if (createProductDto.Price < 0)
            {
                return BadRequest("Product price cannot be negative");
            }

            // Check if product category is valid
            var category = await _productService.isExistCategory(createProductDto.CategoryId);
            if (category == false)
            {
                return BadRequest("Invalid product category");
            }
            var result = await _productService.CreateProductAsync(createProductDto);
            if (result)
            {
                return Ok("Product created successfully");
            }
            else
            {
                return BadRequest("Failed to create product.");
            }
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto productdto)
        {
            var result = await _productService.UpdateProductAsync(id, productdto);
            return Ok(result);
        }
        [HttpPost("{productId}/images")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> UpdateProductImages(int productId, List<IFormFile> images)
        {
            var result = await _productService.UpdateProductImagesAsync(productId, images);
            if(result == true)
            {
                return Ok("Upload Album Image Successfully!!!Ri");
            }
            return BadRequest(false);
            
        }
        [Authorize(Policy = "RequireAdminRole")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
