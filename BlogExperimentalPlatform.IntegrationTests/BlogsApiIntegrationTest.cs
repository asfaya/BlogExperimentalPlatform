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

    public class BlogsApiIntegrationTest : BaseBlogIntegrationTest
    {
        #region Tests

        #region Get
        [Fact]
        public async Task BlogsApi_Get_ReturnBlogDTOCollection()
        {
            // Arrange

            //Act
            var response = await client.GetAsync("api/blogs");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var blogs = JsonConvert.DeserializeObject<ICollection<BlogDTO>>(responseContent);

            blogs.Should().BeAssignableTo<ICollection<BlogDTO>>();
        }

        [Fact]
        public async Task BlogsApi_GetWithId_ReturnBlogDTO()
        {
            // Arrange

            //Act
            var response = await client.GetAsync("api/blogs/1");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var blogs = JsonConvert.DeserializeObject<BlogDTO>(responseContent);

            blogs.Should().BeAssignableTo<BlogDTO>();
        }
        #endregion

        #region Post
        [Fact]
        public async Task BlogsApi_Post_ReturnBlogDTO()
        {
            // Arrange
            var blogDTO = new BlogDTO() { Id = 0, Name = "Test blog", CreationDate = DateTime.MinValue, Entries = new List<BlogEntryDTO>(),
                Deleted = false, Owner = new UserDTO() { Id = 1, UserName = "asfaya", FullName = "Andres Faya", Token = "", Deleted = false }
            };

            string stringData = JsonConvert.SerializeObject(blogDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("api/blogs/", contentData);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var blogs = JsonConvert.DeserializeObject<BlogDTO>(responseContent);

            blogs.Should().BeAssignableTo<BlogDTO>();
        }

        [Fact]
        public async Task BlogsApi_Post_BadParameters_ReturnBadRequest()
        {
            // Arrange
            var blogDTO = new BlogDTO() { Id = 0, Name = "", CreationDate = DateTime.MinValue, Entries = new List<BlogEntryDTO>(), Deleted = false,
                Owner = new UserDTO() { Id = 1, UserName = "asfaya", FullName = "Andres Faya", Token = "", Deleted = false }
            };

            string stringData = JsonConvert.SerializeObject(blogDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("api/blogs/", contentData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Put
        [Fact]
        public async Task BlogsApi_Put_ReturnBlogDTO()
        {
            // Arrange
            var blogDTO = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = new List<BlogEntryDTO>(), Deleted = false,
                Owner = new UserDTO() { Id = 1, UserName = "asfaya", FullName = "Andres Faya", Token = "", Deleted = false }
            };

            string stringData = JsonConvert.SerializeObject(blogDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync("api/blogs/1", contentData);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var blogs = JsonConvert.DeserializeObject<BlogDTO>(responseContent);

            blogs.Should().BeAssignableTo<BlogDTO>();
        }

        [Fact]
        public async Task BlogsApi_Put_NotOwner_ReturnForbidden()
        {
            // Arrange
            var blogDTO = new BlogDTO() { Id = 2, Name = "Test blog", CreationDate = DateTime.Now, Entries = new List<BlogEntryDTO>(), Deleted = false,
                Owner = new UserDTO() { Id = 2, UserName = "asfaya", FullName = "Andres Faya", Token = "", Deleted = false }
            };

            string stringData = JsonConvert.SerializeObject(blogDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync("api/blogs/2", contentData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task BlogsApi_Put_ParametersMismatch_ReturnForbidden()
        {
            // Arrange
            var blogDTO = new BlogDTO() { Id = 2, Name = "Test blog", CreationDate = DateTime.Now, Entries = new List<BlogEntryDTO>(), Deleted = false,
                Owner = new UserDTO() { Id = 2, UserName = "asfaya", FullName = "Andres Faya", Token = "", Deleted = false }
            };

            string stringData = JsonConvert.SerializeObject(blogDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync("api/blogs/10", contentData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task BlogsApi_Put_NotExistent_ReturnBadRequest()
        {
            // Arrange
            var blogDTO = new BlogDTO() { Id = 10, Name = "Test blog", CreationDate = DateTime.Now, Entries = new List<BlogEntryDTO>(), Deleted = false,
                Owner = new UserDTO() { Id = 2, UserName = "asfaya", FullName = "Andres Faya", Token = "", Deleted = false }
            };

            string stringData = JsonConvert.SerializeObject(blogDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync("api/blogs/10", contentData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task BlogsApi_Delete_ReturnNoContent()
        {
            // Arrange
            
            //Act
            var response = await client.DeleteAsync("api/blogs/3");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task BlogsApi_Delete_NotOwner_ReturnForbidden()
        {
            // Arrange

            //Act
            var response = await client.DeleteAsync("api/blogs/2");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task BlogsApi_Delete_NotExistent_ReturnBadRequest()
        {
            // Arrange

            //Act
            var response = await client.DeleteAsync("api/blogs/10");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #endregion
    }
}
