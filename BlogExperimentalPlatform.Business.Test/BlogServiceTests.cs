namespace BlogExperimentalPlatform.Business.Test
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.ServiceImplementations;
    using Moq;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class BlogServiceTests
    {
        #region Members
        private readonly Mock<IBlogRepository> blogRepositoryMock;
        #endregion

        #region Constructor
        public BlogServiceTests()
        {
            blogRepositoryMock = new Mock<IBlogRepository>();
        }
        #endregion

        #region Tests
        [Fact]
        public async Task AddOrUpdateAsync_WhenExisting_ReturnsBlog()
        {
            // Arrange
            var blog = new Blog()
            {
                Id = 1,
                Name = "Test Blog",
                OwnerId = 1,
                CreationDate = DateTime.Now,
                Entries = null,
                Deleted = false
            };

            blogRepositoryMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<Blog>()))
                .ReturnsAsync(blog);

            var service = GetService();

            // Act
            var result = await service.AddOrUpdateAsync(blog);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(blog.Id, result.Id);
            Assert.Equal(blog.Name, result.Name);
            Assert.Equal(blog.OwnerId, result.OwnerId);
            Assert.Equal(blog.CreationDate, result.CreationDate);
            Assert.Equal(blog.Deleted, result.Deleted);

            blogRepositoryMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<Blog>()), Times.Once);
        }

        [Fact]
        public async Task AddOrUpdateAsync_WhenNew_ReturnsBlogWithId()
        {
            // Arrange
            var blog = new Blog()
            {
                Id = 0,
                Name = "Test Blog",
                OwnerId = 1,
                CreationDate = DateTime.MinValue,
                Entries = null,
                Deleted = false
            };

            var expectedCreationDateTime = DateTime.Now;

            blogRepositoryMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<Blog>()))
                .ReturnsAsync(() => {
                    blog.Id = 1;
                    blog.CreationDate = expectedCreationDateTime;
                    return blog;
                });

            var service = GetService();

            // Act
            var result = await service.AddOrUpdateAsync(blog);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(blog.Name, result.Name);
            Assert.Equal(expectedCreationDateTime, result.CreationDate);
            Assert.Equal(blog.OwnerId, result.OwnerId);
            Assert.Equal(blog.CreationDate, result.CreationDate);
            Assert.Equal(blog.Deleted, result.Deleted);

            blogRepositoryMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<Blog>()), Times.Once);
        }
        #endregion

        #region Private Helper Methods
        private BlogService GetService()
        {
            var service = new BlogService(blogRepositoryMock.Object);

            return service;
        }
        #endregion
    }
}
