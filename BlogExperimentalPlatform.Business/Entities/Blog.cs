namespace BlogExperimentalPlatform.Business.Entities
{
    using System;
    using System.Collections.Generic;

    public class Blog : Entity
    {
        public string Name { get; set; }

        public User Owner { get; set; }

        public DateTime CreationDate { get; set; }

        public ICollection<BlogEntry> Entries { get; set; }
    }
}
