using JoyCase.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace JoyCase.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly JoyDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(JoyDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<TResult> SelectOneAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking().Where(filter);
            TResult result = await query.Select(selector).SingleOrDefaultAsync();
            return result;
        }
        public async Task<List<TResult>> SelectAsync<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selector)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking().Where(filter);
            return await query.Select(selector)
                              .ToListAsync();
        }


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }


        //public async Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(string storedProcedure, params object[] parameters) where TResult : class
        //{
        //    return await _context.Database.SqlQueryRaw<TResult>(storedProcedure, parameters).ToListAsync();
        //}

        //public async Task<IEnumerable<TResult>> ExecuteStoredProcedureAsync<TResult>(string storedProcedure, params object[] parameters) where TResult : class
        //{
        //    var results = new List<TResult>();

        //    await using var command = _context.Database.GetDbConnection().CreateCommand();
        //    command.CommandText = storedProcedure;
        //    command.CommandType = System.Data.CommandType.StoredProcedure;

        //    if (parameters != null)
        //    {
        //        foreach (var param in parameters)
        //        {
        //            var dbParam = command.CreateParameter();
        //            dbParam.Value = param;
        //            command.Parameters.Add(dbParam);
        //        }
        //    }

        //    await _context.Database.OpenConnectionAsync();
        //    await using var reader = await command.ExecuteReaderAsync();

        //    var resultType = typeof(TResult);
        //    var properties = resultType.GetProperties();

        //    while (await reader.ReadAsync())
        //    {
        //        var obj = Activator.CreateInstance<TResult>();

        //        foreach (var prop in properties)
        //        {
        //            if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
        //            {
        //                prop.SetValue(obj, reader.GetValue(reader.GetOrdinal(prop.Name)));
        //            }
        //        }

        //        results.Add(obj);
        //    }

        //    await _context.Database.CloseConnectionAsync();
        //    return results;
        //}

        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedure) where T : class
        {
            return await _context.Database.SqlQueryRaw<T>($"EXEC {storedProcedure}").ToListAsync();
        }


    }
}
