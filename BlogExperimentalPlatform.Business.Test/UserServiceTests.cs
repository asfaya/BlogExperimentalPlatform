namespace BlogExperimentalPlatform.Business.Test
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.ServiceImplementations;
    using BlogExperimentalPlatform.Utils;
    using Moq;
    using System.Threading.Tasks;
    using Xunit;

    public class UserServiceTests
    {
        #region Members
        private readonly Mock<IUserRepository> userRepositoryMock;
        #endregion

        #region Constructor
        public UserServiceTests()
        {
            userRepositoryMock = new Mock<IUserRepository>();
        }
        #endregion

        #region Tests

        #region AuthenticateAsync
        [Fact]
        public async Task AuthenticateAsync_WhenCorrect_ReturnsUser()
        {
            // Arrange
            var user = new User() { Id = 1, UserName = "jdoe", FullName = "John Doe", Password = "d41e98d1eafa6d6011d3a70f1a5b92f0", Deleted = false };

            userRepositoryMock
                .Setup(m => m.GetUserByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var service = GetService();

            // Act
            var result = await service.AuthenticateAsync("jdoe", "Passw0rd");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.FullName, result.FullName);
            Assert.Equal(user.Password, result.Password);
            Assert.Equal(user.Deleted, result.Deleted);

            userRepositoryMock.Verify(m => m.GetUserByUserNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_WhenNotExists_ThrowsException()
        {
            // Arrange
            var user = new User() { Id = 1, UserName = "jdoe", FullName = "John Doe", Password = "xxxxxx", Deleted = false };

            userRepositoryMock
                .Setup(m => m.GetUserByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var message = "Incorrect user name or password.";

            var service = GetService();

            // Act
            var exception = await Record.ExceptionAsync(() => service.AuthenticateAsync("jdoe", "password"));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BlogSystemException>(exception);
            Assert.Equal(message, exception.Message);
            userRepositoryMock.Verify(m => m.GetUserByUserNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task AuthenticateAsync_WhenPasswordIncorrect_ThrowsException()
        {
            // Arrange
            User nullUser = null;

            userRepositoryMock
                .Setup(m => m.GetUserByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync(nullUser);

            var message = "Incorrect user name or password.";

            var service = GetService();

            // Act
            var exception = await Record.ExceptionAsync(() => service.AuthenticateAsync("jdoe", "password"));

            // Assert
            Assert.NotNull(exception);
            Assert.IsType<BlogSystemException>(exception);
            Assert.Equal(message, exception.Message);

            userRepositoryMock.Verify(m => m.GetUserByUserNameAsync(It.IsAny<string>()), Times.Once);
        }
        #endregion

        #region GetUserByUserNameAsync
        [Fact]
        public async Task GetUserByUserNameAsync_WhenExists_ReturnsUser()
        {
            // Arrange
            var user = new User() { Id = 1, UserName = "jdoe", FullName = "John Doe", Password = "xxxxxx", Deleted = false };

            userRepositoryMock
                .Setup(m => m.GetUserByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            var service = GetService();

            // Act
            var result = await service.GetUserByUserNameAsync("jdoe");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.UserName, result.UserName);
            Assert.Equal(user.FullName, result.FullName);
            Assert.Equal(user.Password, result.Password);
            Assert.Equal(user.Deleted, result.Deleted);

            userRepositoryMock.Verify(m => m.GetUserByUserNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task GetUserByUserNameAsync_WhenNotExists_ReturnsNull()
        {
            // Arrange
            User nullUser = null;

            userRepositoryMock
                .Setup(m => m.GetUserByUserNameAsync(It.IsAny<string>()))
                .ReturnsAsync(nullUser);

            var service = GetService();

            // Act
            var result = await service.GetUserByUserNameAsync("jdoe");

            // Assert
            Assert.Null(result);

            userRepositoryMock.Verify(m => m.GetUserByUserNameAsync(It.IsAny<string>()), Times.Once);
        }
        #endregion

        #endregion

        #region Private Helper Methods
        private UserService GetService()
        {
            var service = new UserService(userRepositoryMock.Object);

            return service;
        }
        #endregion
    }
}
