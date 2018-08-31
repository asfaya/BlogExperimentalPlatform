namespace BlogExperimentalPlatform.Business.Repositories
{
    using BlogExperimentalPlatform.Business.Entities;
    using System.Threading.Tasks;

    /// <summary>
    /// Repository interface for Users
    /// </summary>
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByUserNameAsync(string userName);
    }
}
