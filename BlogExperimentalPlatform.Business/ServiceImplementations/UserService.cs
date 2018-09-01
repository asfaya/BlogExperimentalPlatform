namespace BlogExperimentalPlatform.Business.ServiceImplementations
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using BlogExperimentalPlatform.Business.Services;
    using BlogExperimentalPlatform.Utils;
    using System.Threading.Tasks;

    public class UserService : BaseService<User>, IUserService
    {
        #region Constructor
        public UserService(IUserRepository repository)
            : base(repository)
        {
        }
        #endregion

        #region Methods
        public async Task<User> AuthenticateAsync(string userName, string password)
        {
            var user = await this.GetUserByUserNameAsync(userName);

            if (user == null)
                throw new BlogSystemException("Incorrect user name or password.");

            if (password.MD5Hash() != user.Password)
                throw new BlogSystemException("Incorrect user name or password.");

            return user;
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await ((IUserRepository)Repository).GetUserByUserNameAsync(userName);
        }
        #endregion
    }
}
