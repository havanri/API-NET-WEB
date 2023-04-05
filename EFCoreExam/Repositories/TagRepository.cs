using EFCoreExam.Data;
using EFCoreExam.Models;

namespace EFCoreExam.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
