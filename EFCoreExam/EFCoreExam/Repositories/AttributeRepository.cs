using EFCoreExam.Data;
using Attribute = EFCoreExam.Models.Attribute;

namespace EFCoreExam.Repositories
{
    public class AttributeRepository : GenericRepository<Attribute>, IAttributeRepository
    {
        public AttributeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
