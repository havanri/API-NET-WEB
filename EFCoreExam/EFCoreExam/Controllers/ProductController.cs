using EFCoreExam.Models;
using EFCoreExam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        [Authorize(Policy = "RequireManagerRole")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.getFullInForAllProduct();
            if (products == null)
            {
                return NoContent();
            }
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Bỏ qua chu trình đối tượng
                MaxDepth = 32 // Giới hạn độ sâu của đối tượng
            };
            var json = JsonConvert.SerializeObject(products, settings);
            return Ok(json);
        }
        [HttpGet("{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.getProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore, // Bỏ qua chu trình đối tượng
                MaxDepth = 32 // Giới hạn độ sâu của đối tượng
            };
            var json = JsonConvert.SerializeObject(product, settings);
            return Ok(json);
        }

        [HttpPost]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> Add([FromForm] CreateProductDto createProductDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductDto productdto)
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
