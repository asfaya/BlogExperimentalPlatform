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
            // User
            modelBuilder.Entity<User>().ToTable("Users").HasKey(r => r.Id);
            modelBuilder.Entity<User>().Property(r => r.Id).UseSqlServerIdentityColumn().IsRequired();
            modelBuilder.Entity<User>().Property(r => r.UserName).IsRequired();
            modelBuilder.Entity<User>().Property(r => r.FullName).IsRequired();
            modelBuilder.Entity<User>().HasQueryFilter(p => !p.Deleted);

            // Blog
            modelBuilder.Entity<Blog>().ToTable("Blogs").HasKey(b => b.Id);
            modelBuilder.Entity<Blog>().Property(b => b.Id).UseSqlServerIdentityColumn().IsRequired();
            modelBuilder.Entity<Blog>().Property(b => b.Name).IsRequired();
            modelBuilder.Entity<Blog>().Property(b => b.CreationDate).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<Blog>().HasOne<User>(b => b.Owner);
            modelBuilder.Entity<Blog>().HasQueryFilter(p => !p.Deleted);

            // BlogEntry
            modelBuilder.Entity<BlogEntry>().ToTable("BlogEntries").HasKey(be => be.Id);
            modelBuilder.Entity<BlogEntry>().Property(be => be.Id).UseSqlServerIdentityColumn().IsRequired();
            modelBuilder.Entity<BlogEntry>().Property(be => be.Title).IsRequired();
            modelBuilder.Entity<BlogEntry>().Property(be => be.Content).IsRequired();
            modelBuilder.Entity<BlogEntry>().HasOne<Blog>(be => be.Blog).WithMany(b => b.Entries);
            modelBuilder.Entity<BlogEntry>().Property(be => be.CreationDate).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<BlogEntry>().Property(be => be.LastUpdated).HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<BlogEntry>().Property(e => e.Status).HasConversion(
                v => v.ToString(),
                v => (BlogEntryStatus)Enum.Parse(typeof(BlogEntryStatus), v));
            modelBuilder.Entity<BlogEntry>().HasMany<BlogEntryUpdate>(be => be.EntryUpdates);
            modelBuilder.Entity<BlogEntry>().HasQueryFilter(p => !p.Deleted);

            // BlogEntryUpdate
            modelBuilder.Entity<BlogEntryUpdate>().ToTable("BlogEntryUpdates").HasKey(beu => beu.Id);
            modelBuilder.Entity<BlogEntryUpdate>().Property(beu => beu.Id).UseSqlServerIdentityColumn().IsRequired();
            modelBuilder.Entity<BlogEntryUpdate>().Property(beu => beu.UpdateMoment).IsRequired().HasDefaultValue(DateTime.Now);
            modelBuilder.Entity<BlogEntryUpdate>().HasQueryFilter(p => !p.Deleted);
        }
        #endregion
    }
}
