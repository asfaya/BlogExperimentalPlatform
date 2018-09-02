namespace BlogExperimentalPlatform.Data.Test
{
    using BlogExperimentalPlatform.Business.Entities;

    public class BaseEntityImplementation : Entity
    {
        public BaseRelatedEntityImplementation RelatedEntity { get; set; }

        public int RelatedEntityId { get; set; }
    }
}
