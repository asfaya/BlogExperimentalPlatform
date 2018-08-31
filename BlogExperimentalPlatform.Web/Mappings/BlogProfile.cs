namespace BlogExperimentalPlatform.Web.Mappings
{
    using AutoMapper;
    using BlogExperimentalPlatform.Business.Entities;
    using BlogExperimentalPlatform.Web.DTOs;

    public class BlogProfile : Profile
    {
        public BlogProfile()
        {
            CreateMap<User, UserDTO>().ForMember(u => u.Token, opt => opt.Ignore());
            CreateMap<UserDTO, User>().ForMember(u => u.Password, opt => opt.Ignore());

            CreateMap<Blog, BlogDTO>();
            CreateMap<BlogDTO, Blog>().ForMember(b => b.Owner, opt => opt.Ignore());

            CreateMap<BlogEntry, BlogEntryDTO>();
            CreateMap<BlogEntryDTO, BlogEntry>().ForMember(b => b.Blog, opt => opt.Ignore());

            CreateMap<BlogEntryUpdate, BlogEntryUpdateDTO>().ReverseMap();
        }
    }
}
