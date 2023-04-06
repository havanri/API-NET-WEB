using EFCoreExam.Data;
using EFCoreExam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace EFCoreExam.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        protected ApplicationDbContext _context;
        private string _errorMessage = string.Empty;
        private bool _isDisposed;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }
        public async Task Update(int Id, T entity)
        {
            var existingEntity = await _context.Set<T>().FindAsync(Id);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
        }
        public virtual void SetEntryModified(T entity)
        {
            _context.Entry(entity).CurrentValues.SetValues(entity);
            _context.SaveChangesAsync();
        }
        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Where(expression);
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
        public void BulkInsert(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
            _context.SaveChanges();
        }

        public void BulkDelete(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            _context.SaveChanges();
        }
        public IEnumerable<T> GetPage(int pageSize, int pageIndex, out int totalItem, Func<T, bool> predicate = null)
        {
            var query = predicate != null ? _context.Set<T>().Where(predicate) : _context.Set<T>();
            totalItem = query.Count();
            var items = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return items;
        }
    }
}
