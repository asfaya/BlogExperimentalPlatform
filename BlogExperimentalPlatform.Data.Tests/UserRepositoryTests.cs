namespace BlogExperimentalPlatform.Data.Test
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using Xunit;
    using FluentAssertions;

    public class UserRepositoryTests
    {
        #region Tests
        [Fact]
        public async Task GetUserByUserNameAsync_WhenExists_ReturnsUser()
        {
            // Arrange
            var user = new User() { Id = 1, UserName = "jdoe", FullName = "John Doe", Password = "d41e98d1eafa6d6011d3a70f1a5b92f0", Deleted = false };

            var context = GetContext();
            context.Users.Add(user);
            context.SaveChanges();
            var repository = GetRepository(context);

            // Act
            var result = await repository.GetUserByUserNameAsync("jdoe");

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(user, opts => opts.IncludingProperties());
        }

        [Fact]
        public async Task GetUserByUserNameAsync_WhenNotExists_ReturnsUser()
        {
            // Arrange
            var context = GetContext();
            var repository = GetRepository(context);

            // Act
            var result = await repository.GetUserByUserNameAsync("asfaya");

            // Assert
            Assert.Null(result);
        }
        #endregion

        #region Private Helper Methods
        private BlogDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<BlogDbContext>()
                                .UseInMemoryDatabase(databaseName: "BlogPOC")
                                .Options;
            return new BlogDbContext(options);
        }

        private UserRepository GetRepository(BlogDbContext context)
        {
            return new UserRepository(context);
        }
        #endregion
    }
}
