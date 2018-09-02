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

    public class BlogsControllerTests
    {
        #region Members
        private readonly Mock<IBlogService> blogServiceMock;
        private readonly Mock<IUserService> userServiceMock;
        private readonly IMapper mapper;
        #endregion

        #region Constructor
        public BlogsControllerTests()
        {
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
        
        #region Get
        [Fact]
        public async Task Get_WhenExists_ReturnCollection()
        {
            // Arrange
            var blog = new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, OwnerId = 1, Entries = new List<BlogEntry>(), Deleted = false,
                                    Owner = new User() { Id = 1, UserName = "jdoe", FullName = "John Doe", Password = "xxxxxx", Deleted = false }
            };

            ICollection<Blog> blogs = new List<Blog> { blog };

            blogServiceMock
                .Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Blog, object>>>()))
                .ReturnsAsync(blogs);

            var expectedReturnCollection = mapper.Map<ICollection<BlogDTO>>(blogs);
            var controller = GetController(mapper);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlogs = Assert.IsAssignableFrom<ICollection<BlogDTO>>(okResult.Value);
            returnBlogs.Should().BeEquivalentTo(expectedReturnCollection, opts => opts.IncludingProperties().IncludingNestedObjects());

            blogServiceMock.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Blog, object>>>()), Times.Once);
        }

        [Fact]
        public async Task Get_WhenNotExists_ReturnEmptyCollection()
        {
            // Arrange
            ICollection<Blog> blogs = new List<Blog>();

            blogServiceMock
                .Setup(m => m.GetAllAsync(It.IsAny<Expression<Func<Blog, object>>>()))
                .ReturnsAsync(blogs);

            var expectedReturnCollection = mapper.Map<ICollection<BlogDTO>>(blogs);
            var controller = GetController(mapper);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlogs = Assert.IsAssignableFrom<ICollection<BlogDTO>>(okResult.Value);
            returnBlogs.Should().BeEquivalentTo(expectedReturnCollection, opts => opts.IncludingProperties().IncludingNestedObjects());

            blogServiceMock.Verify(m => m.GetAllAsync(It.IsAny<Expression<Func<Blog, object>>>()), Times.Once);
        }

        [Fact]
        public async Task Get_WithIdWhenExists_ReturnsBlogDTO()
        {
            // Arrange
            var blog = new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, OwnerId = 1, Entries = new List<BlogEntry>(), Deleted = false,
                Owner = new User() { Id = 1, UserName = "jdoe", FullName = "John Doe", Password = "xxxxxx", Deleted = false }
            };

            blogServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<Blog, object>>>()))
                .ReturnsAsync(blog);

            var expectedReturnValue = mapper.Map<BlogDTO>(blog);
            var controller = GetController(mapper);

            // Act
            var result = await controller.Get(5);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlog = Assert.IsAssignableFrom<BlogDTO>(okResult.Value);
            returnBlog.Should().BeEquivalentTo(expectedReturnValue, opts => opts.IncludingProperties().IncludingNestedObjects());

            blogServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<Blog, object>>>()), Times.Once);
        }

        [Fact]
        public async Task Get_WithIdWhenNotExists_ReturnsNotFound()
        {
            // Arrange
            Blog serviceReturnBlog = null;

            blogServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<Blog, object>>>()))
                .ReturnsAsync(serviceReturnBlog);

            var controller = GetController(mapper);

            // Act
            var result = await controller.Get(5);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            
            blogServiceMock.Verify(m => m.GetAsync(It.IsAny<int>(), It.IsAny<Expression<Func<Blog, object>>>()), Times.Once);
        }
        #endregion

        #region Post
        [Fact]
        public async Task Post_WhenSuccessful_ReturnsBlogDTO ()
        {
            // Arrange
            var blogDTO = new BlogDTO() { Id = 0, Name = "Test blog", CreationDate = DateTime.MinValue, Entries = new List<BlogEntryDTO>(), Deleted = false,
                Owner = new UserDTO() { Id = 1, UserName = "jdoe", FullName = "John Doe", Token = "", Deleted = false }
            };

            var blog = mapper.Map<Blog>(blogDTO);

            blogServiceMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<Blog>()))
                .ReturnsAsync(() =>
                {
                    blog.Id = 1;
                    blog.CreationDate = DateTime.Now;
                    return blog;
                });

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
            var result = await controller.Post(blogDTO);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlog = Assert.IsAssignableFrom<BlogDTO>(okResult.Value);
            Assert.NotEqual(blogDTO.Id, returnBlog.Id);
            Assert.Equal(blogDTO.Name, returnBlog.Name);
            Assert.NotEqual(blogDTO.CreationDate, returnBlog.CreationDate);
            Assert.Equal(blogDTO.Deleted, returnBlog.Deleted);

            blogServiceMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<Blog>()), Times.Once);
        }

        [Fact]
        public async Task Post_WhenException_ThrowsBlogSystemException()
        {
            // Arrange
            var blogDTO = new BlogDTO() { Id = 0, Name = "Test blog", CreationDate = DateTime.MinValue, Entries = new List<BlogEntryDTO>(), Deleted = false,
                Owner = new UserDTO() { Id = 1, UserName = "jdoe", FullName = "John Doe", Token = "", Deleted = false }
            };

            var blog = mapper.Map<Blog>(blogDTO);

            blogServiceMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<Blog>()))
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
            var exception = await Record.ExceptionAsync(() => controller.Post(blogDTO));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BlogSystemException>(exception);
            Assert.Equal(message, exception.Message);

            blogServiceMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<Blog>()), Times.Once);
        }
        #endregion

        #region Put
        [Fact]
        public async Task Put_WhenSuccessful_ReturnsBlogDTO()
        {
            // Arrange
            var blogDTO = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = new List<BlogEntryDTO>(), Deleted = false,
                Owner = new UserDTO() { Id = 1, UserName = "jdoe", FullName = "John Doe", Token = "", Deleted = false }
            };

            var blog = mapper.Map<Blog>(blogDTO);

            blogServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(blog);

            blogServiceMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<Blog>()))
                .ReturnsAsync(blog);

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
            var result = await controller.Put(1, blogDTO);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnBlog = Assert.IsAssignableFrom<BlogDTO>(okResult.Value);
            Assert.Equal(blogDTO.Id, returnBlog.Id);
            Assert.Equal(blogDTO.Name, returnBlog.Name);
            Assert.Equal(blogDTO.CreationDate, returnBlog.CreationDate);
            Assert.Equal(blogDTO.Deleted, returnBlog.Deleted);

            blogServiceMock.Verify(m => m.GetAsync(It.IsAny<int>()), Times.Once);
            blogServiceMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<Blog>()), Times.Once);
        }

        [Fact]
        public async Task Put_WhenUserNotOwner_ReturnsForbidden()
        {
            // Arrange
            var blogDTO = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = new List<BlogEntryDTO>(), Deleted = false,
                Owner = new UserDTO() { Id = 1, UserName = "jdoe", FullName = "John Doe", Token = "", Deleted = false }
            };

            var blog = mapper.Map<Blog>(blogDTO);

            blogServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(blog);

            blogServiceMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<Blog>()))
                .ReturnsAsync(blog);

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
            var result = await controller.Put(1, blogDTO);

            // Assert
            Assert.NotNull(result);
            var forbidResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(403, forbidResult.StatusCode);

            blogServiceMock.Verify(m => m.GetAsync(It.IsAny<int>()), Times.Once);
            blogServiceMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<Blog>()), Times.Never);

        }

        [Fact]
        public async Task Put_WhenException_ThrowsBlogSystemException()
        {
            // Arrange
            var blogDTO = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.MinValue, Entries = new List<BlogEntryDTO>(), Deleted = false,
                Owner = new UserDTO() { Id = 1, UserName = "jdoe", FullName = "John Doe", Token = "", Deleted = false }
            };

            var blog = mapper.Map<Blog>(blogDTO);

            blogServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(blog);

            blogServiceMock
                .Setup(m => m.AddOrUpdateAsync(It.IsAny<Blog>()))
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
            var exception = await Record.ExceptionAsync(() => controller.Put(1, blogDTO));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BlogSystemException>(exception);
            Assert.Equal(message, exception.Message);

            blogServiceMock.Verify(m => m.GetAsync(It.IsAny<int>()), Times.Once);
            blogServiceMock.Verify(m => m.AddOrUpdateAsync(It.IsAny<Blog>()), Times.Once);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task Delete_WhenSuccess_ReturnsNoContent()
        {
            // Arrange
            var blog = new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = new List<BlogEntry>(), Deleted = false, OwnerId = 1 };

            blogServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(blog);

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
            var result = await controller.Delete(1);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
            
            blogServiceMock.Verify(m => m.GetAsync(It.IsAny<int>()), Times.Once);
            blogServiceMock.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Once);

        }

        [Fact]
        public async Task Delete_WhenUserNotOwner_ReturnsForbidden()
        {
            // Arrange
            var blog = new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = new List<BlogEntry>(), Deleted = false, OwnerId = 1 };

            blogServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(blog);

            blogServiceMock
                .Setup(m => m.DeleteAsync(It.IsAny<int>()))
                .Verifiable();

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
            var result = await controller.Delete(1);

            // Assert
            Assert.NotNull(result);
            var forbidResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(403, forbidResult.StatusCode);

            blogServiceMock.Verify(m => m.GetAsync(It.IsAny<int>()), Times.Once);
            blogServiceMock.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Never);

        }

        [Fact]
        public async Task Delete_WhenException_ThrowsBlogSystemException()
        {
            // Arrange
            var blog = new Blog() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = new List<BlogEntry>(), Deleted = false, OwnerId = 1 };

            blogServiceMock
                .Setup(m => m.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(blog);

            blogServiceMock
                .Setup(m => m.DeleteAsync(It.IsAny<int>()))
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

            var message = "There's been an error while trying to delete the blog entry";

            // Act
            var exception = await Record.ExceptionAsync(() => controller.Delete(1));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BlogSystemException>(exception);
            Assert.Equal(message, exception.Message);

            blogServiceMock.Verify(m => m.GetAsync(It.IsAny<int>()), Times.Once);
            blogServiceMock.Verify(m => m.DeleteAsync(It.IsAny<int>()), Times.Once);
        }
        #endregion

        #endregion

        #region Private Helper Methods

        private BlogsController GetController(IMapper mapper)
        {
            return new BlogsController(
                blogServiceMock.Object,
                mapper,
                userServiceMock.Object);
        }
        #endregion

    }


}
