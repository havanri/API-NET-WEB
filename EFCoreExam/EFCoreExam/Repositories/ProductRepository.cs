using EFCoreExam.Data;
using EFCoreExam.Models;
using EFCoreExam.Response;
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
            var listProduct =  _context.Products.Include(e => e.Category).Include(e => e.AlbumImages).
                Include(e => e.ProductTags).ThenInclude(t => t.Tag).Include(e => e.SalePrice).ToList();
            return listProduct;
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            var result = await _context.Products.Include(e => e.Category).Include(e => e.AlbumImages).
                Include(e => e.ProductTags).ThenInclude(t => t.Tag).Include(e => e.SalePrice).
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
