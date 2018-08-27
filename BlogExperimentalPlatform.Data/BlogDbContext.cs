namespace BlogExperimentalPlatform.Data
{
    using BlogExperimentalPlatform.Business.Entities;
    using Microsoft.EntityFrameworkCore;
    using System;

    public class BlogDbContext : DbContext
    {
        #region Constructor
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options) { }
        #endregion

        #region Properties
        public DbSet<User> Users { get; set; }

        public DbSet<Blog> Blogs { get; set; }

        public DbSet<BlogEntry> BlogEntries { get; set; }

        public DbSet<BlogEntryUpdate> BlogEntryUpdates { get; set; }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
        #endregion
    }
}
