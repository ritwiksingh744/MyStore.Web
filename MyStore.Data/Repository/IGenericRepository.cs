using System.Linq.Expressions;

namespace MyStore.Data.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<object> BulkInsertAsync(IEnumerable<T> obj);
        Task<IQueryable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task<object> InsertAsync(T obj);
        Task<object> SaveAsync();
        Task<object> UpdateAsync(T obj, params Expression<Func<T, object>>[] updatedProperties);
        Task<object> BulkUpdateAsync(IEnumerable<T> obj);
        Task<object> BulkDelete(IEnumerable<T> obj);
    }
}