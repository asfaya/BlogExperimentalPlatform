namespace BlogExperimentalPlatform.Web.AutofacConfig
{
    using System.Linq;
    using System.Reflection;
    using Autofac;
    using BlogExperimentalPlatform.Business.ServiceImplementations;

    public class BusinessModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterAssemblyTypes(typeof(BlogService).GetTypeInfo().Assembly)
                .Where(c => c.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}
