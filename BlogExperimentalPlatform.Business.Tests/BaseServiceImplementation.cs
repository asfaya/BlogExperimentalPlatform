namespace BlogExperimentalPlatform.Business.Test
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.ServiceImplementations;

    public class BaseServiceImplementation : BaseService<Entity>
    {
        #region Constructor
        public BaseServiceImplementation(IBaseRepository<Entity> repository) : base(repository) { }
        #endregion
    }
}
