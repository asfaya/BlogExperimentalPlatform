namespace BlogExperimentalPlatform.Data.Test
{
    using Microsoft.EntityFrameworkCore;

    public class BlogDbContextWithEntityImplementation : BlogDbContext
    {
        #region Constructor
        public BlogDbContextWithEntityImplementation(DbContextOptions options) : base(options) { }
        #endregion

        #region Properties
        public DbSet<BaseEntityImplementation> Entities { get; set; }
        #endregion

        #region Methods
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User
            modelBuilder.Entity<BaseEntityImplementation>().ToTable("BaseEntity").HasKey(r => r.Id);
            modelBuilder.Entity<BaseEntityImplementation>().Property(r => r.Id).UseSqlServerIdentityColumn().IsRequired();
            modelBuilder.Entity<BaseEntityImplementation>().HasQueryFilter(p => !p.Deleted);

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
