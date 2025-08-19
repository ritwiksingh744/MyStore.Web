using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MyStore.Data.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly StoreDbContext _context;
        private DbSet<T> _entity;

        public GenericRepository(StoreDbContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }

        public async Task<IQueryable<T>> GetAllAsync()
        {
            return  _entity.AsQueryable();
        }

        public async Task<T> GetByIdAsync(object id)
        {
            return await _entity.FindAsync(id);
        }

        public async Task<object> InsertAsync(T obj)
        {
            await _entity.AddAsync(obj);
            return await SaveAsync();

        }

        public async Task<object> BulkInsertAsync(IEnumerable<T> obj)
        {
            await _entity.AddRangeAsync(obj);
            return await SaveAsync();
        }

        public async Task<object> UpdateAsync(T obj, params Expression<Func<T, object>>[] updatedProperties)
        {
            _entity.Attach(obj);
            if (updatedProperties != null && updatedProperties.Any()) // update partially
            {
                foreach (var property in updatedProperties)
                {
                    _context.Entry(obj).Property(property).IsModified = true;
                }
            }
            else
            {
                _context.Entry(obj).State = EntityState.Modified; // Full update
            }

            return await SaveAsync();
        }

        public async Task<object> BulkUpdateAsync(IEnumerable<T> obj)
        {
            _entity.UpdateRange(obj);
            return await SaveAsync();
        }

        public async Task<object> BulkDelete(IEnumerable<T> obj)
        {
            _entity.RemoveRange(obj);
            return await SaveAsync();
        }

        public async Task<object> SaveAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
