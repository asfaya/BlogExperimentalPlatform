namespace BlogExperimentalPlatform.Web.DTOs
{
    using System;
    using System.Collections.Generic;

    public class BlogDTO : EntityDTO
    {
        public string Name { get; set; }

        public UserDTO Owner { get; set; }

        public DateTime CreationDate { get; set; }

        public ICollection<BlogEntryDTO> Entries { get; set; }
    }
}
