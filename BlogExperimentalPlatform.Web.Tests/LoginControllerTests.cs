namespace BlogExperimentalPlatform.Web.Tests
{
    using AutoMapper;
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Services;
    using BlogExperimentalPlatform.Utils;
    using BlogExperimentalPlatform.Web.Controllers;
    using BlogExperimentalPlatform.Web.DTOs;
    using BlogExperimentalPlatform.Web.Mappings;
    using BlogExperimentalPlatform.Web.Settings;
    using FluentAssertions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class LoginControllerTests
    {
        #region Members
        private readonly Mock<IUserService> userServiceMock;
        private readonly IMapper mapper;
        #endregion

        #region Constructor
        public LoginControllerTests()
        {
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
            var user = new User() { Id = 1, UserName = "jdoe", FullName = "John Doe", Password = "xxxxxx", Deleted = false };

            ICollection<User> users = new List<User> { user };

            userServiceMock
                .Setup(m => m.GetAllAsync())
                .ReturnsAsync(users);

            var expectedReturnCollection = mapper.Map<ICollection<UserDTO>>(users);
            var controller = GetController(mapper);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUsers = Assert.IsAssignableFrom<ICollection<UserDTO>>(okResult.Value);
            returnUsers.Should().BeEquivalentTo(expectedReturnCollection);

            userServiceMock.Verify(m => m.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_WhenNotExists_ReturnEmptyCollection()
        {
            // Arrange
            ICollection<User> users = new List<User>();

            userServiceMock
                .Setup(m => m.GetAllAsync())
                .ReturnsAsync(users);

            var expectedReturnCollection = mapper.Map<ICollection<UserDTO>>(users);
            var controller = GetController(mapper);

            // Act
            var result = await controller.Get();

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUsers = Assert.IsAssignableFrom<ICollection<UserDTO>>(okResult.Value);
            returnUsers.Should().BeEquivalentTo(expectedReturnCollection);

            userServiceMock.Verify(m => m.GetAllAsync(), Times.Once);
        }
        #endregion

        #region Authenticate
        [Fact]
        public async Task Authenticate_WhenUserOk_ReturnUserWithToken()
        {
            // Arrange
            var login = new LoginDTO() { UserName = "jdoe", Password = "Passw0rd" };
            var user = new User() { Id = 1, UserName = "jdoe", FullName = "John Doe", Deleted = false, Password = "d41e98d1eafa6d6011d3a70f1a5b92f0" };
            var expectedReturnUser = mapper.Map<UserDTO>(user);

            userServiceMock
                .Setup(m => m.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(user);

            var controller = GetController(mapper);

            // Act
            var result = await controller.Authenticate(login);

            // Assert
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUser = Assert.IsType<UserDTO>(okResult.Value);
            Assert.Equal(returnUser.Id, expectedReturnUser.Id);
            Assert.Equal(returnUser.UserName, expectedReturnUser.UserName);
            Assert.Equal(returnUser.FullName, expectedReturnUser.FullName);
            Assert.NotEqual(string.Empty, expectedReturnUser.Token);

            userServiceMock.Verify(m => m.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Authenticate_WhenCantAuthenticate_ReturnUnauthorized()
        {
            // Arrange
            var login = new LoginDTO() { UserName = "jdoe", Password = "Passw0rd" };
            User serviceReturnUser = null;

            userServiceMock
                .Setup(m => m.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(serviceReturnUser);

            var controller = GetController(mapper);

            // Act
            var result = await controller.Authenticate(login);

            // Assert
            Assert.NotNull(result);
            var badRequest = Assert.IsType<UnauthorizedResult>(result);

            userServiceMock.Verify(m => m.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
        #endregion

        #endregion

        #region Private Helper Methods

        private LoginController GetController(IMapper mapper)
        {
            // Options
            IOptions<SecuritySettings> settings = Options.Create<SecuritySettings>(new SecuritySettings()
            {
                TokenTimeOut = 60,
                Issuer = "BlogExperimentalPlatform.Security.Bearer",
                Audience = "BlogExperimentalPlatform.Security.Bearer",
                Secret = "# BlogExperimentalPlatformUltraTopSecret123!!@ "
            });

            return new LoginController(
                settings,
                mapper,
                userServiceMock.Object);
        }
        #endregion
    }
}
