namespace BlogExperimentalPlatform.Data.Repositories
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;

    public class BlogRepository : BaseRepository<Blog>, IBlogRepository
    {
        #region Constructor
        public BlogRepository(BlogDbContext ctx) : base(ctx)
        {
        }
        #endregion
    }
}
