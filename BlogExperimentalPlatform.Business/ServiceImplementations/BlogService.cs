namespace BlogExperimentalPlatform.Business.ServiceImplementations
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.Services;
    using System;
    using System.Threading.Tasks;

    public class BlogService : BaseService<Blog>, IBlogService
    {
        #region Constructor
        public BlogService(IBlogRepository repository)
            : base(repository)
        {
        }
        #endregion

        #region Methods
        public override Task<Blog> AddOrUpdateAsync(Blog entity)
        {
            if (entity.Id == 0)
                entity.CreationDate = DateTime.Now;

            return base.AddOrUpdateAsync(entity);
        }
        #endregion
    }
}
