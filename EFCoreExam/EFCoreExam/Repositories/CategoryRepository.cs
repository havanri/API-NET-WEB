using EFCoreExam.Data;
using EFCoreExam.Models;

namespace EFCoreExam.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
