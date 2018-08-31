namespace BlogExperimentalPlatform.Business.ServiceImplementations
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.Services;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class BlogEntryService : BaseService<BlogEntry>, IBlogEntryService
    {
        #region Constructor
        public BlogEntryService(IBlogEntryRepository repository)
            : base(repository)
        {
        }
        #endregion

        #region Methods
        public override Task<BlogEntry> AddOrUpdateAsync(BlogEntry entity)
        {
            if (entity.Id == 0)
                entity.CreationDate = DateTime.Now;
            entity.LastUpdated = DateTime.Now;
            if (entity.EntryUpdates == null)
                entity.EntryUpdates = new List<BlogEntryUpdate>();
            entity.EntryUpdates.Add(new BlogEntryUpdate
            {
                Id = 0,
                UpdateMoment = entity.LastUpdated
            });

            return base.AddOrUpdateAsync(entity);
        }
        #endregion
    }
}
