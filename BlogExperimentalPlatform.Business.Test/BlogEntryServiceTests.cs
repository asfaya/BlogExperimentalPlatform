namespace BlogExperimentalPlatform.Business.Test
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.ServiceImplementations;
    using Moq;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class BlogEntryServiceTests
    {
        #region Members
        private readonly Mock<IBlogEntryRepository> blogEntryRepositoryMock;
        #endregion

        #region Constructor
        public BlogEntryServiceTests()
        {
            blogEntryRepositoryMock = new Mock<IBlogEntryRepository>();
        }
        #endregion

        #region Tests
        [Fact]
        public async Task AddOrUpdateAsync_WhenExisting_ReturnsBlogEntry()
        {
            // Arrange
            var blogEntry = new BlogEntry()
            {
                Id = 1,
                Title = "Test Blog Entry",
                Content = "Test Blog Entry Content",
                BlogId = 1,
                CreationDate = DateTime.Now,
                LastUpdated = DateTime.MinValue,
                EntryUpdates = null,
                Status = BlogEntryStatus.Public,
                Deleted = false
            };

            var resultBlogEntry = new BlogEntry()
            {
                Id = blogEntry.Id,
                Title = blogEntry.Title,
                Content = blogEntry.Content,
                BlogId = blogEntry.BlogId,
                CreationDate = blogEntry.CreationDate,
                LastUpdated = DateTime.Now,
                EntryUpdates = null,
                Status = blogEntry.Status,
                Deleted = blogEntry.Deleted
            };

            blogEntryRepositoryMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()))
                .ReturnsAsync(resultBlogEntry);

            var service = GetService();

            // Act
            var result = await service.AddOrUpdateAsync(blogEntry);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(blogEntry.Id, result.Id);
            Assert.Equal(blogEntry.Title, result.Title);
            Assert.Equal(blogEntry.Content, result.Content);
            Assert.Equal(blogEntry.BlogId, result.BlogId);
            Assert.Equal(blogEntry.CreationDate, result.CreationDate);
            Assert.NotEqual(blogEntry.LastUpdated, result.LastUpdated);
            Assert.Equal(blogEntry.Status, result.Status);
            Assert.Equal(blogEntry.Deleted, result.Deleted);

            blogEntryRepositoryMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()), Times.Once);
        }

        [Fact]
        public async Task AddOrUpdateAsync_WhenNew_ReturnsBlogEntryWithId()
        {
            // Arrange
            var blogEntry = new BlogEntry()
            {
                Id = 0,
                Title = "Test Blog Entry",
                Content = "Test Blog Entry Content",
                BlogId = 1,
                CreationDate = DateTime.MinValue,
                LastUpdated = DateTime.MinValue,
                EntryUpdates = null,
                Status = BlogEntryStatus.Public,
                Deleted = false
            };

            var expectedCreationDateTime = DateTime.Now;

            blogEntryRepositoryMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()))
                .ReturnsAsync(() => {
                    blogEntry.Id = 1;
                    blogEntry.CreationDate = blogEntry.LastUpdated = expectedCreationDateTime;
                    return blogEntry;
                });

            var service = GetService();

            // Act
            var result = await service.AddOrUpdateAsync(blogEntry);

            // Assert
            Assert.NotNull(result);
            Assert.NotEqual(0, result.Id);
            Assert.Equal(blogEntry.Title, result.Title);
            Assert.Equal(blogEntry.Content, result.Content);
            Assert.Equal(blogEntry.BlogId, result.BlogId);
            Assert.Equal(expectedCreationDateTime, result.CreationDate);
            Assert.Equal(expectedCreationDateTime, result.LastUpdated);
            Assert.Equal(blogEntry.Status, result.Status);
            Assert.Equal(blogEntry.Deleted, result.Deleted);

            blogEntryRepositoryMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()), Times.Once);
        }
        #endregion

        #region Private Helper Methods
        private BlogEntryService GetService()
        {
            var service = new BlogEntryService(blogEntryRepositoryMock.Object);

            return service;
        }
        #endregion
    }
}
