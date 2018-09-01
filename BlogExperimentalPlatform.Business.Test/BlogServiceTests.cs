namespace BlogExperimentalPlatform.Business.Test
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.ServiceImplementations;
    using FluentAssertions;
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
                CreationDate = DateTime.MinValue,
                Entries = null,
                Deleted = false
            };

            var resultBlog = new Blog()
            {
                Id = blog.Id,
                Name = blog.Name,
                OwnerId = blog.OwnerId,
                CreationDate = blog.CreationDate,
                Entries = null,
                Deleted = blog.Deleted
            };

            blogRepositoryMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<Blog>()))
                .ReturnsAsync(resultBlog);

            var service = GetService();

            // Act
            var result = await service.AddOrUpdateAsync(blog);

            // Assert
            Assert.NotNull(result);
            result.Should().BeEquivalentTo(blog);

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

            var resultBlog = new Blog()
            {
                Id = 1,
                Name = blog.Name,
                OwnerId = blog.OwnerId,
                CreationDate = DateTime.Now,
                Entries = null,
                Deleted = false
            };
            
            blogRepositoryMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<Blog>()))
                .ReturnsAsync(resultBlog);

            var service = GetService();

            // Act
            var result = await service.AddOrUpdateAsync(blog);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(blog.Id, result.Id);
            Assert.Equal(blog.Name, result.Name);
            Assert.NotEqual(DateTime.MinValue, result.CreationDate);
            Assert.Equal(blog.OwnerId, result.OwnerId);
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
