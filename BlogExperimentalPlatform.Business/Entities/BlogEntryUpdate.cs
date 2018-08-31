namespace BlogExperimentalPlatform.Business.Entities
{
    using System;

    public class BlogEntryUpdate : Entity
    {
        public int BlogEntryId { get; set; }

        public DateTime UpdateMoment { get; set; }
    }
}
