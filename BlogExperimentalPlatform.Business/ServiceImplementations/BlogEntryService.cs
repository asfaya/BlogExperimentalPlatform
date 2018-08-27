namespace BlogExperimentalPlatform.Business.ServiceImplementations
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.Services;

    public class BlogEntryService : BaseService<BlogEntry>, IBlogEntryService
    {
        #region Constructor
        public BlogEntryService(IBlogEntryRepository repository)
            : base(repository)
        {
        }
        #endregion
    }
}
