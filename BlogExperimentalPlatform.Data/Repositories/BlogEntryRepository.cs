

namespace BlogExperimentalPlatform.Data.Repositories
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class BlogEntryRepository : BaseRepository<BlogEntry>, IBlogEntryRepository
    {
        #region Constructor
        public BlogEntryRepository(BlogDbContext ctx) : base(ctx)
        {
        }
        #endregion

        #region Members
        public override async Task<BlogEntry> AddOrUpdateAsync(BlogEntry entity)
        {
            BlogContext.ChangeTracker.TrackGraph(entity, e =>
            {
                if (e.Entry.Entity is BlogEntry || e.Entry.Entity is BlogEntryUpdate)
                {
                    if ((e.Entry.Entity as Entity).Id > 0)
                        e.Entry.State = EntityState.Modified;
                    else
                        e.Entry.State = EntityState.Added;
                }
                else
                {
                    e.Entry.State = EntityState.Unchanged;
                }
            });

            await BlogContext.SaveChangesAsync();

            return await BlogContext.BlogEntries.FindAsync(entity.Id);
        }
        #endregion
    }
}
