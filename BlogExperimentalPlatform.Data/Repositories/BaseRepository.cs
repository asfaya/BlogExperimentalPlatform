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
                .AsNoTracking();

            return await query.ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = BlogContext.Set<T>()
                .AsNoTracking();

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return await query.ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetPaginatedAsync(int currentPage, int pageSize)
        {
            IQueryable<T> query = BlogContext.Set<T>()
                .AsNoTracking();

            return await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize).ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetPaginatedAsync(int currentPage, int pageSize, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = BlogContext.Set<T>()
                .AsNoTracking();

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize).ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetPaginatedAsync(int currentPage, int pageSize, Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = BlogContext.Set<T>()
                .AsNoTracking()
                .Where(predicate);

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize).ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetPaginatedAsync(int currentPage, int pageSize, Expression<Func<T, bool>> predicate, Expression<Func<T, object>> sortCondition, bool sortDesc, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = BlogContext.Set<T>()
                .AsNoTracking()
                .Where(predicate);

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            if (sortDesc)
                query = query.OrderByDescending(sortCondition);
            else
                query = query.OrderBy(sortCondition);

            return await query
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize).ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> query = BlogContext.Set<T>()
                .AsNoTracking()
                .Where(predicate);

            return await query.ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetFilteredAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = BlogContext.Set<T>()
                .AsNoTracking()
                .Where(predicate);

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return await query.ToListAsync();
        }

        public virtual async Task<int> CountAsync()
        {
            return await BlogContext.Set<T>().AsNoTracking().CountAsync();
        }

        public virtual async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return await BlogContext
                .Set<T>()
                .AsNoTracking()
                .Where(predicate)
                .CountAsync();
        }

        public virtual async Task<T> GetSingleAsync(int id)
        {
            return await BlogContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<T> GetSingleAsync(int id, params Expression<Func<T, object>>[] includeProperties)
        {
            var query = BlogContext.Set<T>().AsNoTracking()
                .Where(t => t.Id == id);
            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            return await query.FirstOrDefaultAsync();
        }

        public virtual async Task<T> AddOrUpdateAsync(T entity)
        {
            // Not for updating full tree graphs
            // Will add / update the entity sent as parameter, not the related ones.
            BlogContext.ChangeTracker.TrackGraph(entity, e =>
            {
                if (e.Entry.Entity == entity)
                {
                    if ((e.Entry.Entity as T).Id > 0)
                        e.Entry.State = EntityState.Modified;
                    else
                        e.Entry.State = EntityState.Added;
                }
                else
                {
                    var trackedEntity = BlogContext.ChangeTracker.Entries().FirstOrDefault(ent => ((Entity)ent.Entity).Id == ((Entity)e.Entry.Entity).Id && ent.Entity.GetType() == e.Entry.Entity.GetType());
                    if (trackedEntity != null && trackedEntity.State == EntityState.Detached)
                    {
                        trackedEntity.State = EntityState.Unchanged;
                    }
                    else
                    {
                        if (trackedEntity == null)
                            e.Entry.State = EntityState.Unchanged;
                    }
                }
            });

            await BlogContext.SaveChangesAsync();

            return await BlogContext.Set<T>().FindAsync(entity.Id);
        }

        public virtual async Task DeleteAsync(int id)
        {
            // Soft delete implementation
            var entity = await BlogContext.Blogs
                .FirstOrDefaultAsync(e => e.Id == id);
            entity.Deleted = true;
            await BlogContext.SaveChangesAsync();
        }
        #endregion
    }
}
