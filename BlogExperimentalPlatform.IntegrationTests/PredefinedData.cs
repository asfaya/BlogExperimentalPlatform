using BlogExperimentalPlatform.Business.Entities;
using System;
using System.Collections.Generic;

namespace BlogExperimentalPlatform.IntegrationTests
{
    public static class PredefinedData
    {
        public static string Password = @"!Covfefe123";

        public static User[] Users = new[] {
            new User { UserName = "asfaya", FullName = "Andres Faya", Password = "d41e98d1eafa6d6011d3a70f1a5b92f0", Deleted = false },
            new User { UserName = "jdoe", FullName = "John Doe", Password = "d41e98d1eafa6d6011d3a70f1a5b92f0", Deleted = false }
        };

        public static Blog[] Blogs = new[] {
            new Blog { Name = "AF Blog", OwnerId = 1, CreationDate = DateTime.Now, Deleted = false, Entries = new List<BlogEntry>() {
                        new BlogEntry() { Title = "First Entry", Content = "First Entry Content", CreationDate = DateTime.Now, LastUpdated = DateTime.Now, Status = BlogEntryStatus.Public, Deleted = false, EntryUpdates = new List<BlogEntryUpdate>() {
                                new BlogEntryUpdate() { UpdateMoment = DateTime.Now }
                        }
                    },
                        new BlogEntry() { Title = "Second Entry", Content = "Second Entry Content", CreationDate = DateTime.Now, LastUpdated = DateTime.Now, Status = BlogEntryStatus.Public, Deleted = false, EntryUpdates = new List<BlogEntryUpdate>() {
                                new BlogEntryUpdate() { UpdateMoment = DateTime.Now }
                        }
                    }
                }
            },
            new Blog { Name = "JD Blog", OwnerId = 2, CreationDate = DateTime.Now, Deleted = false, Entries = new List<BlogEntry>() {
                        new BlogEntry() { Title = "Third Entry", Content = "Third Entry Content", CreationDate = DateTime.Now, LastUpdated = DateTime.Now, Status = BlogEntryStatus.Public, Deleted = false, EntryUpdates = new List<BlogEntryUpdate>() {
                                new BlogEntryUpdate() { UpdateMoment = DateTime.Now }
                        }
                    },
                        new BlogEntry() { Title = "Fourth Entry", Content = "Fourth Entry Content", CreationDate = DateTime.Now, LastUpdated = DateTime.Now, Status = BlogEntryStatus.Public, Deleted = false, EntryUpdates = new List<BlogEntryUpdate>() {
                                new BlogEntryUpdate() { UpdateMoment = DateTime.Now }
                        }
                    }
                }
            }
        };
    }
}
