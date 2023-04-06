
using EFCoreExam.Data;
using EFCoreExam.Models;

namespace EFCoreExam.Repositories
{
    public class AlbumImageRepository : GenericRepository<AlbumImage>, IAlbumImageRepository
    {
        public AlbumImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}


