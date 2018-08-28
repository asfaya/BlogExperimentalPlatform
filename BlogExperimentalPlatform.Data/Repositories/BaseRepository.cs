namespace BlogExperimentalPlatform.Data.Repositories
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : Entity
    {
        #region Members
        private readonly BlogDbContext blogContext;
        #endregion

        #region Constructor
        public BaseRepository(BlogDbContext ctx)
        {
            this.blogContext = ctx ?? throw new ArgumentNullException("Error on BlogDbContext DI");
        }
        #endregion

        #region Properties
        protected BlogDbContext BlogContext => blogContext;
        #endregion

        #region IBaseRepository Methods
        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            IQueryable<T> query = BlogContext.Set<T>()
                .AsNoTracking()
                .Where(t => t.Deleted == false);

            return await query.ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = BlogContext.Set<T>()
                .AsNoTracking()
                .Where(t => t.Deleted == false);

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return await query.ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = BlogContext.Set<T>()
                .AsNoTracking()
                .Where(t => t.Deleted == false)
                .Where(predicate);

            return await query.ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = BlogContext.Set<T>()
                .AsNoTracking()
                .Where(t => t.Deleted == false)
                .Where(predicate);

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return await query.ToListAsync();
        }

        public virtual async Task<T> GetSingleAsync(int id)
        {
            return await BlogContext.Set<T>()
                .Where(t => t.Deleted == false)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<T> GetSingleAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = BlogContext.Set<T>().AsNoTracking()
                .Where(t => t.Id == id && t.Deleted == false);
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<T> AddOrUpdateAsync(T entity)
        {
            // Not for updating tree graphs
            // Need to override for that
            var existingItem = await BlogContext.Set<T>().FindAsync(entity.Id);
            if (existingItem == null)
                await BlogContext.Set<T>().AddAsync(entity);
            else
                BlogContext.Entry(existingItem).CurrentValues.SetValues(entity);

            await BlogContext.SaveChangesAsync();

            return await BlogContext.Set<T>().FindAsync(entity.Id);
        }

        public virtual async Task DeleteAsync(int id)
        {
            // Soft delete implementation
            var entity = await GetSingleAsync(id);
            entity.Deleted = true;
            await BlogContext.SaveChangesAsync();
        }
        #endregion
    }
}
