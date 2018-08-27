namespace BlogExperimentalPlatform.Business.ServiceImplementations
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.Services;

    public class BlogService : BaseService<Blog>, IBlogService
    {
        #region Constructor
        public BlogService(IBlogRepository repository)
            : base(repository)
        {
        }
        #endregion
    }
}
