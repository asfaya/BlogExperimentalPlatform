namespace BlogExperimentalPlatform.IntegrationTests
{
    using BlogExperimentalPlatform.Web.Security;
    using BlogExperimentalPlatform.Web.Settings;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Extensions.Configuration;
    using System.IO;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Text.RegularExpressions;

    public abstract class BaseBlogIntegrationTest
    {
        #region Properties
        protected readonly TestServer testServer;
        protected readonly HttpClient client;

        protected readonly string authorization = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhc2ZheWEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYXNmYXlhIiwianRpIjoiYjBkNDYyNjEtYWE5OC00OWI3LTg2OGEtY2FmNjI4NDUxNjYxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvc2lkIjoiMSIsImV4cCI6MTUzNTkwNjg3MywiaXNzIjoiQmxvZ0V4cGVyaW1lbnRhbFBsYXRmb3JtLlNlY3VyaXR5LkJlYXJlciIsImF1ZCI6IkJsb2dFeHBlcmltZW50YWxQbGF0Zm9ybS5TZWN1cml0eS5CZWFyZXIifQ.qVltJc-7TOL1mvcOd8xQEUcsifftGlwflm5ou0UkQn4";
        #endregion

        #region Constructor
        public BaseBlogIntegrationTest() 
        {
            var integrationTestsPath = GetApplicationRoot();
            var applicationPath = Path.GetFullPath(Path.Combine(integrationTestsPath, "../BlogExperimentalPlatform.Web"));
            
            var configuration = new ConfigurationBuilder()
                    .SetBasePath(applicationPath)
                    .AddJsonFile("appsettings.json")
                    .Build();

            var jwtConfig = new SecuritySettings();

            configuration
                .GetSection("SecuritySettings")
                .Bind(jwtConfig);

            testServer = new TestServer(
                WebHost.CreateDefaultBuilder()
                .UseStartup<TestStartup>()
                .UseContentRoot(applicationPath)
                .UseConfiguration(configuration));

            client = testServer.CreateClient();
            this.authorization = "Bearer " + this.GetBearerToken(1, "asfaya", jwtConfig.Secret, jwtConfig.Issuer, jwtConfig.Audience, jwtConfig.TokenTimeOut);
            client.DefaultRequestHeaders.Add("Authorization", authorization);
        }
        #endregion

        #region Helper Methods
        protected string GetBearerToken(int id, string subject, string secret, string issuer, string audience, int expiry)
        {
            var token = new JwtTokenBuilder()
                                .AddSecurityKey(JwtSecurityKey.Create(secret))
                                .AddSubject(subject)
                                .AddClaim(ClaimTypes.Sid, id.ToString())
                                .AddIssuer(issuer)
                                .AddAudience(audience)
                                .AddExpiry(expiry)
                                .Build();

            return token.Value;
        }
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
