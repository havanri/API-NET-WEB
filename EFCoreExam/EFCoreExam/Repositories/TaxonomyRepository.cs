using EFCoreExam.Data;
using EFCoreExam.Models;

namespace EFCoreExam.Repositories
{
    public class TaxonomyRepository : GenericRepository<Taxonomy>, ITaxonomyRepository
    {
        public TaxonomyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
