﻿

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
    }
}
