using EFCoreExam.Models;

namespace EFCoreExam.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> getFullInForAllProduct();
        Task<Product> GetProductByNameAsync(string productName);
        Task<Product> GetProductByIdAsync(int productId);
    }
}
