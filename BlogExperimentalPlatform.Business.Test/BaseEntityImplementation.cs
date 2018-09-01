namespace BlogExperimentalPlatform.Business.Test
{
    using BlogExperimentalPlatform.Business.Entities;

    public class BaseEntityImplementation : Entity
    {
        public BaseEntityImplementation RelatedEntity { get; set; }
    }
}
