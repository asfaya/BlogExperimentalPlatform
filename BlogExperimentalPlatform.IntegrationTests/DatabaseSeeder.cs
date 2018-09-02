namespace BlogExperimentalPlatform.IntegrationTests
{
    using BlogExperimentalPlatform.Data;
    using System;
    using System.Threading.Tasks;

    public class DatabaseSeeder
    {
        private readonly BlogDbContext context;

        public DatabaseSeeder(BlogDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException("BlogDbContext DI injection error");
        }

        public void Seed()
        {
            // Add all the predefined DB information
            context.Users.AddRange(PredefinedData.Users);
            context.Blogs.AddRange(PredefinedData.Blogs);
            context.SaveChanges();
        }
    }
}
