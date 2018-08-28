namespace BlogExperimentalPlatform.Web.Mappings
{
    using AutoMapper;
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Web.DTOs;

    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Blog, BlogDTO>().ReverseMap();
            CreateMap<BlogEntry, BlogEntryDTO>().ReverseMap();
            CreateMap<BlogEntryUpdate, BlogEntryUpdateDTO>().ReverseMap();
        }
    }
}
