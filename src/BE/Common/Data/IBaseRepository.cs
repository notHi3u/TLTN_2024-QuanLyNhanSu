using System.Linq.Expressions;

namespace Common.Data
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T?> GetByIdAsync(object id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> DeleteListAsync(IEnumerable<T> entities);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
