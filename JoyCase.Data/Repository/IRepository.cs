using System.Linq.Expressions;

namespace JoyCase.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<TResult> SelectOneAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector);
        Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector);

        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<int> SaveChangesAsync();

        Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedure) where T : class;
    }
}
