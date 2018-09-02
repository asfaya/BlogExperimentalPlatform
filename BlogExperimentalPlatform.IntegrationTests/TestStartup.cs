namespace BlogExperimentalPlatform.IntegrationTests
{
    using AutoMapper;
    using AutoMapper.Configuration;
    using BlogExperimentalPlatform.Data;
    using BlogExperimentalPlatform.Web;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Reflection;

    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env) : base(env)
        {
        }

        protected override void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddApplicationPart(Assembly.Load(new AssemblyName("BlogExperimentalPlatform.Web")));
        }

        protected override void ConfigureDatabases(IServiceCollection services)
        {
            // Create a new service provider.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Add a database context (ApplicationDbContext) using an in-memory 
            // database for testing.
            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
                options.UseInternalServiceProvider(serviceProvider);
            });

            // Build the service provider.
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database
            // context (ApplicationDbContext).
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<BlogDbContext>();

                // Ensure the database is created.
                db.Database.EnsureCreated();

                try
                {
                    // Seed the database with test data.
                    var seeder = new DatabaseSeeder(db);
                    seeder.Seed();
                    //Utilities.InitializeDbForTests(db);
                }
                catch 
                {
                }
            }
        }

        protected override void ConfigureHttps(IApplicationBuilder app)
        {
        }

        protected override void ConfigureSpa(IApplicationBuilder app, IHostingEnvironment env)
        {
        }
    }
}
