namespace BlogExperimentalPlatform.Web.AutofacConfig
{
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using BlogExperimentalPlatform.Data;
    using BlogExperimentalPlatform.Data.Repositories;

    public class DataModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BlogDbContext>().As<BlogDbContext>().InstancePerLifetimeScope();

            builder
                .RegisterAssemblyTypes(typeof(BlogRepository).GetTypeInfo().Assembly)
                .Where(c => c.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
