namespace BlogExperimentalPlatform.Web
{
    using System;
    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using AutoMapper;
    using BlogExperimentalPlatform.Data;
    using BlogExperimentalPlatform.Web.AutofacConfig;
    using BlogExperimentalPlatform.Web.DTOs;
    using BlogExperimentalPlatform.Web.Middlewares;
    using BlogExperimentalPlatform.Web.Security;
    using BlogExperimentalPlatform.Web.Settings;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public System.IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Registering configuration information to be used by container
            services.AddOptions();

            // Add settings from configuration
            services.Configure<SecuritySettings>(Configuration.GetSection("SecuritySettings"));

            // Configure security
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                   .AddJwtBearer(options =>
                   {
                       options.TokenValidationParameters =
                            new TokenValidationParameters
                            {
                                ValidateIssuer = true,
                                ValidateAudience = true,
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ClockSkew = TimeSpan.Zero,
                                ValidIssuer = Configuration["SecuritySettings:Issuer"],
                                ValidAudience = Configuration["SecuritySettings:Audience"],
                                IssuerSigningKey = JwtSecurityKey.Create(Configuration["SecuritySettings:Secret"])
                            };
                   });

            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });

            ConfigureDatabases(services);

            // AutoMapper
            services.AddAutoMapper();

            // Add MVC
            ConfigureMvc(services);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            // Add Autofac
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<BusinessModule>();
            containerBuilder.RegisterModule<DataModule>();

            containerBuilder.Populate(services);
            var container = containerBuilder.Build();

            return container.Resolve<System.IServiceProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, IMapper mapper)
        {
            // As for use of the middleware
            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            // }
            // else
            // {
            //     app.UseExceptionHandler("/Error");
            //     app.UseHsts();
            // }
            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            ConfigureHttps(app);
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            mapper.ConfigurationProvider.AssertConfigurationIsValid();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            ConfigureSpa(app, env);
        }

        protected virtual void ConfigureMvc(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        protected virtual void ConfigureDatabases(IServiceCollection services)
        {
            // Add EF core services.
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<BlogDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("Blog")));
        }

        protected virtual void ConfigureHttps(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
        }

        protected virtual void ConfigureSpa(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSpa(spa =>
            {
                /* To learn more about options for serving an Angular SPA from ASP.NET Core,
                   see https://go.microsoft.com/fwlink/?linkid=864501 */

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
