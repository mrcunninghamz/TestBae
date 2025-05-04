using AutoMapper;
using TestBae.Tests.Mocks.Blogging.DTOs;
using TestBae.Tests.Mocks.Blogging.Entities;

namespace TestBae.Tests.Mocks.Blogging.Profiles;

public class BlogProfile : Profile
{
    public BlogProfile()
    {
        // Blog mappings
        CreateMap<Blog, BlogDto>();
        CreateMap<CreateBlogDto, Blog>()
            .ForMember(b => b.BlogId, opt => opt.Ignore())
            .ForMember(b => b.Posts, opt => opt.Ignore());
        CreateMap<UpdateBlogDto, Blog>()
            .ForMember(b => b.Posts, opt => opt.Ignore());
        
        // Post mappings
        CreateMap<Post, PostDto>();
        CreateMap<CreatePostDto, Post>()
            .ForMember(b => b.PostId, opt => opt.Ignore())
            .ForMember(b => b.Blog, opt => opt.Ignore());
        CreateMap<UpdatePostDto, Post>()
            .ForMember(b => b.BlogId, opt => opt.Ignore())
            .ForMember(b => b.Blog, opt => opt.Ignore());
    }
}
