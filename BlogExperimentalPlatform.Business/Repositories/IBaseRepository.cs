namespace BlogExperimentalPlatform.Business.Repositories
{
    using BlogExperimentalPlatform.Business.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Base repository
    /// It's aim is to provide the methods that all repos will have to provide.
    /// Particular methods for different repositories will be defined on their own 
    /// interface
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IBaseRepository<T> where T : Entity
    {
        Task<ICollection<T>> GetAllAsync();

        Task<ICollection<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);

        Task<ICollection<T>> GetPaginatedAsync(int currentPage, int pageSize);

        Task<ICollection<T>> GetPaginatedAsync(int currentPage, int pageSize, params Expression<Func<T, object>>[] includeProperties);

        Task<ICollection<T>> GetPaginatedAsync(int currentPage, int pageSize, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<ICollection<T>> GetPaginatedAsync(int currentPage, int pageSize, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> sortCondition, bool sortDesc, params Expression<Func<T, object>>[] includeProperties);

        Task<ICollection<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate);

        Task<ICollection<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<int> CountAsync();

        Task<int> CountAsync(Expression<Func<T, bool>> predicate);

        Task<T> GetSingleAsync(int id);

        Task<T> GetSingleAsync(int id, params Expression<Func<T, object>>[] includeProperties);

        Task<T> AddOrUpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
