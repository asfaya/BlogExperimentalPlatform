namespace BlogExperimentalPlatform.IntegrationTests
{
    using BlogExperimentalPlatform.Business.Entities;
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

    public class BlogEntriesApiIntegrationTest : BaseBlogIntegrationTest
    {
        #region Tests

        #region GetAllByBlogId
        [Fact]
        public async Task BlogEntriesApi_GetAllByBlogId_ReturnBlogDTOCollection()
        {
            // Arrange

            //Act
            var response = await client.GetAsync("api/blogentries/GetAllByBlogId/1");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var blogs = JsonConvert.DeserializeObject<ICollection<BlogEntryDTO>>(responseContent);

            blogs.Should().BeAssignableTo<ICollection<BlogEntryDTO>>();
        }
        #endregion

        #region Get
        [Fact]
        public async Task BlogEntriesApi_GetWithId_ReturnBlogEntryDTO()
        {
            // Arrange

            //Act
            var response = await client.GetAsync("api/blogentries/1");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var blogs = JsonConvert.DeserializeObject<BlogEntryDTO>(responseContent);

            blogs.Should().BeAssignableTo<BlogEntryDTO>();
        }
        #endregion

        #region Post
        [Fact]
        public async Task BlogEntriesApi_Post_ReturnBlogEntryDTO()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 0, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.MinValue, LastUpdated = DateTime.MinValue, Deleted = false,
                Blog = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            string stringData = JsonConvert.SerializeObject(blogEntryDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("api/blogentries/", contentData);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var blogs = JsonConvert.DeserializeObject<BlogEntryDTO>(responseContent);

            blogs.Should().BeAssignableTo<BlogEntryDTO>();
        }

        [Fact]
        public async Task BlogEntriesApi_Post_BadParameters_ReturnBadRequest()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 0, Title = "", Content = "", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.MinValue, LastUpdated = DateTime.MinValue, Deleted = false,
                Blog = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            string stringData = JsonConvert.SerializeObject(blogEntryDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("api/blogentries/", contentData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task BlogEntriesApi_Post_NotOwner_ReturnForbidden()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 0, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.MinValue, LastUpdated = DateTime.MinValue, Deleted = false,
                Blog = new BlogDTO() { Id = 2, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            string stringData = JsonConvert.SerializeObject(blogEntryDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PostAsync("api/blogentries/", contentData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }
        #endregion

        #region Put
        [Fact]
        public async Task BlogEntriesApi_Put_ReturnBlogEntryDTO()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 1, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.MinValue, LastUpdated = DateTime.MinValue, Deleted = false,
                Blog = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            string stringData = JsonConvert.SerializeObject(blogEntryDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync("api/blogentries/1", contentData);

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var blogs = JsonConvert.DeserializeObject<BlogEntryDTO>(responseContent);

            blogs.Should().BeAssignableTo<BlogEntryDTO>();
        }

        [Fact]
        public async Task BlogEntriesApi_Put_NotOwner_ReturnForbidden()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 3, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.MinValue, LastUpdated = DateTime.MinValue, Deleted = false,
                Blog = new BlogDTO() { Id = 2, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            string stringData = JsonConvert.SerializeObject(blogEntryDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync("api/blogentries/3", contentData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task BlogEntriesApi_Put_ParametersMismatch_ReturnForbidden()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 1, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.MinValue, LastUpdated = DateTime.MinValue, Deleted = false,
                Blog = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            string stringData = JsonConvert.SerializeObject(blogEntryDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync("api/blogentries/10", contentData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task BlogEntriesApi_Put_NotExistent_ReturnBadRequest()
        {
            // Arrange
            var blogEntryDTO = new BlogEntryDTO() { Id = 10, Title = "Title", Content = "Content", EntryUpdates = new List<BlogEntryUpdateDTO>(), Status = BlogEntryStatus.Public,
                CreationDate = DateTime.MinValue, LastUpdated = DateTime.MinValue, Deleted = false,
                Blog = new BlogDTO() { Id = 1, Name = "Test blog", CreationDate = DateTime.Now, Entries = null, Deleted = false, Owner = null }
            };

            string stringData = JsonConvert.SerializeObject(blogEntryDTO);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync("api/blogentries/10", contentData);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #region Delete
        [Fact]
        public async Task BlogEntriesApi_Delete_ReturnNoContent()
        {
            // Arrange

            //Act
            var response = await client.DeleteAsync("api/blogentries/2");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task BlogEntriesApi_Delete_NotOwner_ReturnForbidden()
        {
            // Arrange

            //Act
            var response = await client.DeleteAsync("api/blogentries/3");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task BlogEntriesApi_Delete_NotExistent_ReturnBadRequest()
        {
            // Arrange

            //Act
            var response = await client.DeleteAsync("api/blogentries/10");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
        #endregion

        #endregion
    }
}
