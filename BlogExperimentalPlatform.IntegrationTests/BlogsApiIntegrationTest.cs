namespace BlogExperimentalPlatform.IntegrationTests
{
    using BlogExperimentalPlatform.Web.DTOs;
    using FluentAssertions;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Xunit;


    public class BlogsApiIntegrationTest
    {
        #region Properties
        private readonly TestServer testServer;
        private readonly HttpClient client;

        public object PlatformServices { get; }

        private string authorizarion = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhc2ZheWEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYXNmYXlhIiwianRpIjoiYjBkNDYyNjEtYWE5OC00OWI3LTg2OGEtY2FmNjI4NDUxNjYxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiMSIsImV4cCI6MTUzNTkwNjg3MywiaXNzIjoiQmxvZ0V4cGVyaW1lbnRhbFBsYXRmb3JtLlNlY3VyaXR5LkJlYXJlciIsImF1ZCI6IkJsb2dFeHBlcmltZW50YWxQbGF0Zm9ybS5TZWN1cml0eS5CZWFyZXIifQ.qVltJc-7TOL1mvcOd8xQEUcsifftGlwflm5ou0UkQn4";
        #endregion

        #region Constructor
        public BlogsApiIntegrationTest()
        {
            var integrationTestsPath = GetApplicationRoot(); 
            var applicationPath = Path.GetFullPath(Path.Combine(integrationTestsPath, "../BlogExperimentalPlatform.Web"));

            testServer = new TestServer(
                WebHost.CreateDefaultBuilder()
                .UseStartup<TestStartup>()
                .UseContentRoot(applicationPath)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(applicationPath) 
                    .AddJsonFile("appsettings.json")
                    .Build()));

            client = testServer.CreateClient();
            client.DefaultRequestHeaders.Add("Authorization", authorizarion);
        }
        #endregion

        #region Tests
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
        public async Task BlogsApi_Delete_ReturnBlogDTO()
        {
            // Arrange
            
            //Act
            var response = await client.DeleteAsync("api/blogs/1");

            // Assert
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
        #endregion

        #region Helper Methods
        private string GetApplicationRoot()
        {
            var exePath = Path.GetDirectoryName(System.Reflection
                              .Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return appRoot;
        }
        #endregion
    }
}
