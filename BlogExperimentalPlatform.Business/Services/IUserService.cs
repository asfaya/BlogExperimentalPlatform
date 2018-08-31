namespace BlogExperimentalPlatform.Business.Services
{
    using BlogExperimentalPlatform.Business.Entities;
    using System.Threading.Tasks;

    /// <summary>
    /// Service interface for Users
    /// </summary>
    public interface IUserService : IBaseService<User>
    {
        Task<User> AuthenticateAsync(string userName, string password);

        Task<User> GetUserByUserNameAsync(string userName);
    }
}
