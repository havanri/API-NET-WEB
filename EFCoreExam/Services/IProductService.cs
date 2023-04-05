using EFCoreExam.Models;

namespace EFCoreExam.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> getFullInForAllProduct();
        Task<IEnumerable<Product>> getAll();
        Task<bool> CreateProductAsync(CreateProductDto createProductDto);
        Task<bool> isExistCategory(int categoryId);
        Task<bool> UpdateProductAsync(int id, UpdateProductDto updateProductDto);
        Task<bool> UpdateProductImagesAsync(int productId, List<IFormFile> images);
        Task DeleteProduct(int productId);
    }
}
