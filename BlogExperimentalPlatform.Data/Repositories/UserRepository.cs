namespace BlogExperimentalPlatform.Data.Repositories
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Business.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;

    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        #region Constructor
        public UserRepository(BlogDbContext ctx) : base(ctx)
        {
        }
        #endregion

        #region Methods
        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await BlogContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserName == userName);
        }
        #endregion
    }
}
