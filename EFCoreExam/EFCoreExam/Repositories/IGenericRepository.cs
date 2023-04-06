using EFCoreExam.Models;
using System.Linq.Expressions;

namespace EFCoreExam.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> expression);
        Task Add(T entity);
        Task Update(int Id, T entity);
        void Delete(T entity);
        void BulkDelete(IEnumerable<T> entities);
        void BulkInsert(IEnumerable<T> entities);
        IEnumerable<T> GetPage(int pageSize, int pageIndex, out int totalItem, Func<T, bool> predicate = null);
    }
}
