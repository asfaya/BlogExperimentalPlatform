namespace BlogExperimentalPlatform.Business.ServiceImplementations
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.Services;

    public class UserService : BaseService<User>, IUserService
    {
        #region Constructor
        public UserService(IUserRepository repository)
            : base(repository)
        {
        }
        #endregion
    }
}
