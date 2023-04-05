using EFCoreExam.Data;
using EFCoreExam.Models;
using Microsoft.EntityFrameworkCore;

namespace EFCoreExam.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> getFullInForAllProduct()
        {
            var listProduct =  _context.Products.ToList();
            return listProduct;
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var result = await _context.Products.Include(p => p.AlbumImages).Include(p => p.Category).
                FirstOrDefaultAsync(p => p.Id == productId);
            return result;
        }

        public async Task<Product> GetProductByNameAsync(string productName)
        {
            var result = await _context.Products.Include(p => p.AlbumImages).Include(p => p.Category).
                FirstOrDefaultAsync(p => p.Name == productName);
            return result;
        }
    }
}
