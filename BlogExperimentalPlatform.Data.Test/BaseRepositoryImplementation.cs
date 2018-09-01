namespace BlogExperimentalPlatform.Data.Test
{
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Data.Repositories;

    public class BaseRepositoryImplementation : BaseRepository<BaseEntityImplementation>
    {
        public BaseRepositoryImplementation(BlogDbContextWithEntityImplementation context) : base(context) { }
    }
}
