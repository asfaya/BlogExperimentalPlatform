namespace BlogExperimentalPlatform.Web.Tests
{
    using AutoMapper;
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Services;
    using BlogExperimentalPlatform.Utils;
    using BlogExperimentalPlatform.Web.Controllers;
    using BlogExperimentalPlatform.Web.DTOs;
    using BlogExperimentalPlatform.Web.Mappings;
    using FluentAssertions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Xunit;

    public class BlogEntriesControllerTests
    {
        #region Members
        private readonly Mock<IBlogEntryService> blogEntryServiceMock;
        private readonly Mock<IBlogService> blogServiceMock;
        private readonly Mock<IUserService> userServiceMock;
        private readonly IMapper mapper;
        #endregion

        #region Constructor
        public BlogEntriesControllerTests()
        {
            blogEntryServiceMock = new Mock<IBlogEntryService>();
            blogServiceMock = new Mock<IBlogService>();
            userServiceMock = new Mock<IUserService>();

            //auto mapper configuration
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new BlogProfile());
            });
            mapper = mockMapper.CreateMapper();
        }
        #endregion

        #region Tests

        #region GetAllByBlogId
        [Fact]
        public async Task GetAllByBlogId_WhenExists_ReturnsBlogEntryDTOCollection()
        {
            // Arrange
            var blogEntry = new BlogEntry() { Id = 1, Title = "Title", Content = "Content", Status = BlogEntryStatus.Public, CreationDate = DateTime.Now,
                LastUpdated = DateTime.Now, Deleted = false, BlogId = 1,
                Blog = new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = new List<BlogEntry>(), Deleted = false,
                    OwnerId = 1,
                    Owner = new User() { Id = 1, UserName = "jdoe", FullName = "John Doe", Password = "xxxxxx", Deleted = false }
                }
            };

            var entries = new List<BlogEntry>() { blogEntry };

            blogEntryServiceMock
                .Setup(m => m.GetPaginatedAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Expression<Func<BlogEntry, bool>>>(), 
                    It.IsAny<Expression<Func<BlogEntry, object>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<BlogEntry, object>>>(),
                    It.IsAny<Expression<Func<BlogEntry, object>>>()))
                .ReturnsAsync(new EntityPage<BlogEntry>() { TotalEntities = 1, TotalFiltered = 1, TotalPages = 1, Entities = entries });

            var expectedReturnValue = mapper.Map<ICollection<BlogEntryDTO>>(entries);
            var controller = GetController(mapper);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Pagination"] = "";

            // Act
            var result = await controller.GetAllByBlogId(5);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlogEntry = Assert.IsAssignableFrom<ICollection<BlogEntryDTO>>(okResult.Value);
            returnBlogEntry.Should().BeEquivalentTo(expectedReturnValue, opts => opts.IncludingProperties().IncludingNestedObjects());

            blogEntryServiceMock.Verify(m => m.GetPaginatedAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Expression<Func<BlogEntry, bool>>>(),
                    It.IsAny<Expression<Func<BlogEntry, object>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<BlogEntry, object>>>(),
                    It.IsAny<Expression<Func<BlogEntry, object>>>()), Times.Once);
        }

        [Fact]
        public async Task GetAllByBlogId_WhenNotExists_ReturnsEmptyCollection()
        {
            // Arrange
            var entries = new List<BlogEntry>();

            blogEntryServiceMock
                .Setup(m => m.GetPaginatedAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Expression<Func<BlogEntry, bool>>>(),
                    It.IsAny<Expression<Func<BlogEntry, object>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<BlogEntry, object>>>(),
                    It.IsAny<Expression<Func<BlogEntry, object>>>()))
                .ReturnsAsync(new EntityPage<BlogEntry>() { TotalEntities = 0, TotalFiltered = 0, TotalPages = 1, Entities = entries });

            var expectedReturnValue = mapper.Map<ICollection<BlogEntryDTO>>(entries);
            var controller = GetController(mapper);

            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.Request.Headers["Pagination"] = "";

            // Act
            var result = await controller.GetAllByBlogId(5);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlogEntry = Assert.IsAssignableFrom<ICollection<BlogEntryDTO>>(okResult.Value);
            returnBlogEntry.Should().BeEquivalentTo(expectedReturnValue, opts => opts.IncludingProperties().IncludingNestedObjects());

            blogEntryServiceMock.Verify(m => m.GetPaginatedAsync(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Expression<Func<BlogEntry, bool>>>(),
                    It.IsAny<Expression<Func<BlogEntry, object>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<Expression<Func<BlogEntry, object>>>(),
                    It.IsAny<Expression<Func<BlogEntry, object>>>()), Times.Once);
        }
        #endregion

        #region Get
        [Fact]
        public async Task Get_WithIdWhenExists_ReturnsBlogEntryDTO()
        {
            // Arrange
            var blogEntry = new BlogEntry() { Id = 1, Title = "Title", Content = "Content", Status = BlogEntryStatus.Public, LastUpdated = DateTime.Now, Deleted = false,
                BlogId = 1, Blog = new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = new List<BlogEntry>(),
                    OwnerId = 1, Owner = new User() { Id = 1, UserName = "jdoe", FullName = "John Doe", Password = "xxxxxx", Deleted = false }
                }
            };

            blogEntryServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<BlogEntry, object>>>(), It.IsAny<Expression<Func<BlogEntry, object>>>()))
                .ReturnsAsync(blogEntry);

            var expectedReturnValue = mapper.Map<BlogEntryDTO>(blogEntry);
            var controller = GetController(mapper);

            // Act
            var result = await controller.Get(5);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlogEntry = Assert.IsAssignableFrom<BlogEntryDTO>(okResult.Value);
            returnBlogEntry.Should().BeEquivalentTo(expectedReturnValue, opts => opts.IncludingProperties().IncludingNestedObjects());

            blogEntryServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<BlogEntry, object>>>(), It.IsAny<Expression<Func<BlogEntry, object>>>()), Times.Once);
        }

        [Fact]
        public async Task Get_WithIdWhenNotExists_ReturnsNotFound()
        {
            // Arrange
            BlogEntry serviceReturnBlog = null;

            blogEntryServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<BlogEntry, object>>>(), It.IsAny<Expression<Func<BlogEntry, object>>>()))
                .ReturnsAsync(serviceReturnBlog);

            var controller = GetController(mapper);

            // Act
            var result = await controller.Get(5);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);

            blogEntryServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<BlogEntry, object>>>(), It.IsAny<Expression<Func<BlogEntry, object>>>()), Times.Once);
        }
        #endregion

        #region Post
        [Fact]
        public async Task Post_WhenSuccessful_ReturnsBlogEntryDTO()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 0, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.MinValue, LastUpdated = DateTime.MinValue, Deleted = false,
                Blog = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            var blogEntry = new BlogEntry() { Id = 1, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdate>(), Status = BlogEntryStatus.Public, CreationDate = DateTime.Now,
                LastUpdated = DateTime.Now, Deleted = false, BlogId = 1 
            };

            blogServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, OwnerId = 1, Owner = null });

            blogEntryServiceMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()))
                .ReturnsAsync(blogEntry);

            var controller = GetController(mapper);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "jdoe"),
                        new Claim(ClaimTypes.Sid, "1")
                    }, "jwt"))
                }
            };

            // Act
            var result = await controller.Post(blogEntryDTO);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlogEntry = Assert.IsAssignableFrom<BlogEntryDTO>(okResult.Value);
            Assert.NotEqual(blogEntryDTO.Id, returnBlogEntry.Id);
            Assert.Equal(blogEntryDTO.Title, returnBlogEntry.Title);
            Assert.Equal(blogEntryDTO.Content, returnBlogEntry.Content);
            Assert.NotEqual(blogEntryDTO.CreationDate, returnBlogEntry.CreationDate);
            Assert.NotEqual(blogEntryDTO.LastUpdated, returnBlogEntry.LastUpdated);
            Assert.Equal(blogEntryDTO.Status, returnBlogEntry.Status);
            Assert.Equal(blogEntryDTO.Deleted, returnBlogEntry.Deleted);

            blogServiceMock.Verify(m => m.GetAsync(It.IsAny<int>()), Times.Once);
            blogEntryServiceMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()), Times.Once);
        }

        [Fact]
        public async Task Post_WhenUserNotOwner_ReturnsForbidden()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 0, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.MinValue, LastUpdated = DateTime.MinValue, Deleted = false,
                Blog = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            var blogEntry = new BlogEntry() { Id = 1, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdate>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.Now, LastUpdated = DateTime.Now, Deleted = false, BlogId = 1
            };

            blogServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, OwnerId = 1, Owner = null });

            blogEntryServiceMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()))
                .ReturnsAsync(blogEntry);

            var controller = GetController(mapper);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "jdoe"),
                        new Claim(ClaimTypes.Sid, "2")
                    }, "jwt"))
                }
            };

            // Act
            var result = await controller.Post(blogEntryDTO);

            // Assert
            Assert.NotNull(result);
            var forbidResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(403, forbidResult.StatusCode);

            blogServiceMock.Verify(m => m.GetAsync(It.IsAny<int>()), Times.Once);
            blogEntryServiceMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()), Times.Never);
        }

        [Fact]
        public async Task Post_WhenException_ThrowsBlogSystemException()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 0, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.MinValue, LastUpdated = DateTime.MinValue, Deleted = false,
                Blog = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            blogServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, OwnerId = 1, Owner = null });

            blogEntryServiceMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()))
                .Throws<Exception>();

            var controller = GetController(mapper);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "jdoe"),
                        new Claim(ClaimTypes.Sid, "1")

                    }, "jwt"))
                }
            };

            var message = "There's been an error while trying to add the blog entry";

            // Act
            var exception = await Record.ExceptionAsync(() => controller.Post(blogEntryDTO));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BlogSystemException>(exception);
            Assert.Equal(message, exception.Message);

            blogServiceMock.Verify(m => m.GetAsync(It.IsAny<int>()), Times.Once);
            blogEntryServiceMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()), Times.Once);
        }
        #endregion

        #region Put
        [Fact]
        public async Task Put_WhenSuccessful_ReturnsBlogEntryDTO()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 1, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.Now, LastUpdated = DateTime.Now, Deleted = false,
                Blog = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            var blogEntry = new BlogEntry() { Id = 1, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdate>(), Status = BlogEntryStatus.Public,
                CreationDate = blogEntryDTO.CreationDate, LastUpdated = blogEntryDTO.LastUpdated, BlogId = 1, Deleted = false,
                Blog = new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, OwnerId = 1, Owner = null }
            };

            blogEntryServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<BlogEntry, object>>>()))
                .ReturnsAsync(blogEntry);

            blogEntryServiceMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()))
                .ReturnsAsync(blogEntry);

            var controller = GetController(mapper);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "jdoe"),
                        new Claim(ClaimTypes.Sid, "1")
                    }, "jwt"))
                }
            };

            // Act
            var result = await controller.Put(1, blogEntryDTO);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlogEntry = Assert.IsAssignableFrom<BlogEntryDTO>(okResult.Value);
            Assert.Equal(blogEntryDTO.Id, returnBlogEntry.Id);
            Assert.Equal(blogEntryDTO.Title, returnBlogEntry.Title);
            Assert.Equal(blogEntryDTO.Content, returnBlogEntry.Content);
            Assert.Equal(blogEntryDTO.CreationDate, returnBlogEntry.CreationDate);
            Assert.Equal(blogEntryDTO.LastUpdated, returnBlogEntry.LastUpdated);
            Assert.Equal(blogEntryDTO.Status, returnBlogEntry.Status);
            Assert.Equal(blogEntryDTO.Deleted, returnBlogEntry.Deleted);

            blogEntryServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<BlogEntry, object>>>()), Times.Once);
            blogEntryServiceMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()), Times.Once);
        }

        [Fact]
        public async Task Put_WhenUserNotOwner_ReturnsForbidden()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 1, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.Now, LastUpdated = DateTime.Now, Deleted = false,
                Blog = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            var blogEntry = new BlogEntry() { Id = 1, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdate>(), Status = BlogEntryStatus.Public,
                CreationDate = blogEntryDTO.CreationDate, LastUpdated = blogEntryDTO.LastUpdated, Deleted = false, BlogId = 1,
                Blog = new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, OwnerId = 1, Owner = null }
            };

            blogEntryServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<BlogEntry, object>>>()))
                .ReturnsAsync(blogEntry);

            blogEntryServiceMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()))
                .ReturnsAsync(blogEntry);

            var controller = GetController(mapper);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "jdoe"),
                        new Claim(ClaimTypes.Sid, "2")
                    }, "jwt"))
                }
            };

            // Act
            var result = await controller.Put(1, blogEntryDTO);

            // Assert
            Assert.NotNull(result);
            var forbidResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(403, forbidResult.StatusCode);

            blogEntryServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<BlogEntry, object>>>()), Times.Once);
            blogEntryServiceMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()), Times.Never);
        }

        [Fact]
        public async Task Put_WhenException_ThrowsBlogSystemException()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 1, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.Now, LastUpdated = DateTime.Now, Deleted = false,
                Blog = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            var blogEntry = new BlogEntry() { Id = 1, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdate>(), Status = BlogEntryStatus.Public,
                CreationDate = blogEntryDTO.CreationDate, LastUpdated = blogEntryDTO.LastUpdated, Deleted = false, BlogId = 1,
                Blog = new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, OwnerId = 1, Owner = null }
            };

            blogEntryServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<BlogEntry, object>>>()))
                .ReturnsAsync(blogEntry);

            blogEntryServiceMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()))
                .Throws<Exception>();

            var controller = GetController(mapper);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "jdoe"),
                        new Claim(ClaimTypes.Sid, "1")

                    }, "jwt"))
                }
            };

            var message = "There's been an error while trying to add the blog entry";

            // Act
            var exception = await Record.ExceptionAsync(() => controller.Put(1, blogEntryDTO));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BlogSystemException>(exception);
            Assert.Equal(message, exception.Message);

            blogEntryServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<BlogEntry, object>>>()), Times.Once);
            blogEntryServiceMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<BlogEntry>()), Times.Once);
        }
        #endregion

        #endregion

        #region Private Helper Methods

        private BlogEntriesController GetController(IMapper mapper)
        {
            return new BlogEntriesController(
                blogEntryServiceMock.Object,
                blogServiceMock.Object,
                userServiceMock.Object,
                mapper);
        }
        #endregion
    }
}
