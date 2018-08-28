﻿namespace BlogExperimentalPlatform.Business.ServiceImplementations
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public abstract class BaseService<T> : IBaseService<T>
         where T : Entity
    {
        #region Members
        private readonly IBaseRepository<T> repository;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<T> repository)
        {
            this.repository = repository ?? throw new ArgumentNullException("DI error for repository");
        }
        #endregion

        #region Properties
        protected IBaseRepository<T> Repository => repository;
        #endregion

        #region Methods
        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await Repository.GetAllAsync();
        }

        public virtual async Task<ICollection<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            return await Repository.GetAllAsync(includeProperties);
        }

        public virtual async Task<ICollection<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate)
        {
            return await Repository.GetFilteredAsync(predicate);
        }

        public virtual async Task<ICollection<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            return await Repository.GetFilteredAsync(predicate, includeProperties);
        }

        public virtual async Task<T> GetAsync(int id)
        {
            return await Repository.GetSingleAsync(id);
        }

        public virtual async Task<T> GetAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            return await Repository.GetSingleAsync(id, includeProperties);
        }

        public virtual async Task<T> AddOrUpdateAsync(T entity)
        {
            return await Repository.AddOrUpdateAsync(entity);
        }

        public virtual async Task DeleteAsync(int id)
        {
            await Repository.DeleteAsync(id);
        }
        #endregion
    }
}
