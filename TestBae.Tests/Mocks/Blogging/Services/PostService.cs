using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestBae.Tests.Mocks.Blogging.DTOs;
using TestBae.Tests.Mocks.Blogging.Entities;
using TestBae.Tests.Mocks.Blogging.Interfaces;

namespace TestBae.Tests.Mocks.Blogging.Services;

public class PostService : IPostService
{
    private readonly BloggingContext _context;
    private readonly IMapper _mapper;

    public PostService(BloggingContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<PostDto>> GetAllPostsAsync()
    {
        var posts = await _context.Posts.ToListAsync();
        return _mapper.Map<List<PostDto>>(posts);
    }

    public async Task<List<PostDto>> GetPostsByBlogIdAsync(int blogId)
    {
        var posts = await _context.Posts
            .Where(p => p.BlogId == blogId)
            .ToListAsync();
            
        return _mapper.Map<List<PostDto>>(posts);
    }

    public async Task<PostDto> GetPostByIdAsync(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        return _mapper.Map<PostDto>(post);
    }

    public async Task<PostDto> CreatePostAsync(CreatePostDto createPostDto)
    {
        // Verify blog exists
        var blogExists = await _context.Blogs.AnyAsync(b => b.BlogId == createPostDto.BlogId);
        if (!blogExists)
        {
            return null;
        }
        
        var post = _mapper.Map<Post>(createPostDto);
        
        _context.Posts.Add(post);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<PostDto>(post);
    }

    public async Task<PostDto> UpdatePostAsync(int id, UpdatePostDto updatePostDto)
    {
        var post = await _context.Posts.FindAsync(id);
        
        if (post == null)
        {
            return null;
        }

        _mapper.Map(updatePostDto, post);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<PostDto>(post);
    }

    public async Task<bool> DeletePostAsync(int id)
    {
        var post = await _context.Posts.FindAsync(id);
        
        if (post == null)
        {
            return false;
        }

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        
        return true;
    }
}
