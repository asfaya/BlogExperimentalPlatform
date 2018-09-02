namespace BlogExperimentalPlatform.IntegrationTests
{
    using BlogExperimentalPlatform.Web.DTOs;
    using FluentAssertions;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class LoginApiIntegrationTest : BaseBlogIntegrationTest
    {
        #region Tests

        #region Get
        [Fact]
        public async Task LoginApi_Get_ReturnUserDTOCollection()
        {
            // Arrange

            //Act
            var response = await client.GetAsync("api/login");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var blogs = JsonConvert.DeserializeObject<ICollection<UserDTO>>(responseContent);

            blogs.Should().BeAssignableTo<ICollection<UserDTO>>();
        }
        #endregion

        #region Authenticate
        [Fact]
        public async Task LoginApi_Authenticate_ReturnUserDTOCollection()
        {
            // Arrange
            var loginDTO = new LoginDTO() { UserName = "asfaya", Password = "Passw0rd" };

            string stringData = JsonConvert.SerializeObject(loginDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("api/login", contentData);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var blogs = JsonConvert.DeserializeObject<UserDTO>(responseContent);

            blogs.Should().BeAssignableTo<UserDTO>();
        }

        [Fact]
        public async Task LoginApi_Authenticate_BadCredentials_ReturnBadRequest()
        {
            // Arrange
            var loginDTO = new LoginDTO() { UserName = "asfaya", Password = "OtherPassword" };

            string stringData = JsonConvert.SerializeObject(loginDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("api/login", contentData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginApi_Authenticate_Incomplete_ReturnBadRequest()
        {
            // Arrange
            var loginDTO = new LoginDTO() { UserName = "asfaya", Password = "" };

            string stringData = JsonConvert.SerializeObject(loginDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("api/login", contentData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #endregion
    }
}
