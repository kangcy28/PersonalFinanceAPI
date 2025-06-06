using Dapper;
using Dapper.Contrib.Extensions;
using PersonalFinanceAPI.Models.Entities;
using System.Data;

namespace PersonalFinanceAPI.Data.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly IDbConnectionFactory _connectionFactory;
        protected readonly string _tableName;

        protected BaseRepository(IDbConnectionFactory connectionFactory, string tableName)
        {
            _connectionFactory = connectionFactory;
            _tableName = tableName;
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.GetAsync<T>(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.GetAllAsync<T>();
        }

        public virtual async Task<int> AddAsync(T entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            
            using var connection = _connectionFactory.CreateConnection();
            return await connection.InsertAsync(entity);
        }

        public virtual async Task<bool> UpdateAsync(T entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            
            using var connection = _connectionFactory.CreateConnection();
            return await connection.UpdateAsync(entity);
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            var entity = await GetByIdAsync(id);
            if (entity == null) return false;
            
            return await connection.DeleteAsync(entity);
        }

        protected async Task<IEnumerable<T>> QueryAsync(string sql, object? parameters = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<T>(sql, parameters);
        }

        protected async Task<T?> QuerySingleOrDefaultAsync(string sql, object? parameters = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
        }

        protected async Task<int> ExecuteAsync(string sql, object? parameters = null)
        {
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql, parameters);
        }
    }
}