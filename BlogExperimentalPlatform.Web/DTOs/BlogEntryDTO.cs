namespace BlogExperimentalPlatform.Web.DTOs
{
    using System;
    using System.Collections.Generic;
    using BlogExperimentalPlatform.Business.Entities;

    public class BlogEntryDTO : EntityDTO
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public BlogDTO Blog { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdated { get; set; }

        public BlogEntryStatus Status { get; set; }

        public ICollection<BlogEntryUpdateDTO> EntryUpdates { get; set; }
    }
}
