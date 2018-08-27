namespace BlogExperimentalPlatform.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using BlogExperimentalPlatform.Business.Entities;

    /// <summary>
    /// Base service.
    /// It's aim to define the minimun operations that a service should provide
    /// Any particular operation for services should be definen on their own
    /// interface
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    public interface IBaseService<T> where T : Entity
    {
        Task<T> GetAsync(int id);

        Task<T> GetAsync(int id, params Expression<Func<T, object>>[] includeProperties);

        Task<ICollection<T>> GetAllAsync();

        Task<ICollection<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);

        Task<ICollection<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate);

        Task<ICollection<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task<T> AddOrUpdateAsync(T entity);

        Task DeleteAsync(int id);
    }
}
