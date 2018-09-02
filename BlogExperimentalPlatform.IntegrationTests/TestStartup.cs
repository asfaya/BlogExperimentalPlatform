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
            // Add EF core services.
            services.AddDbContext<BlogDbContext>(options =>
                options.UseInMemoryDatabase("blogplayground_test_db"));

            // Register the database seeder
            services.AddTransient<DatabaseSeeder>();
        }

        public override void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
        {
            // Perform all the configuration in the base class
            base.Configure(app, env, mapper);

            // Now seed the database
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var seeder = serviceScope.ServiceProvider.GetService<DatabaseSeeder>();
                seeder.Seed();
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
