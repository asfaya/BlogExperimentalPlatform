namespace BlogExperimentalPlatform.Business.Entities
{
    using System;
    using System.Collections.Generic;

    public class BlogEntry : Entity
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public Blog Blog { get; set; }

        public int BlogId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdated { get; set; }

        public BlogEntryStatus Status { get; set; }

        public ICollection<BlogEntryUpdate> EntryUpdates { get; set; }
    }
}
