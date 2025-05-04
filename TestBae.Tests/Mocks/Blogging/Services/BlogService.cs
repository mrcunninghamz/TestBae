using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestBae.Tests.Mocks.Blogging.DTOs;
using TestBae.Tests.Mocks.Blogging.Entities;
using TestBae.Tests.Mocks.Blogging.Interfaces;

namespace TestBae.Tests.Mocks.Blogging.Services;

public class BlogService : IBlogService
{
    private readonly BloggingContext _context;
    private readonly IMapper _mapper;

    public BlogService(BloggingContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<BlogDto>> GetAllBlogsAsync()
    {
        var blogs = await _context.Blogs
            .Include(b => b.Posts)
            .ToListAsync();
        
        return _mapper.Map<List<BlogDto>>(blogs);
    }

    public async Task<BlogDto> GetBlogByIdAsync(int id)
    {
        var blog = await _context.Blogs
            .Include(b => b.Posts)
            .FirstOrDefaultAsync(b => b.BlogId == id);
            
        return _mapper.Map<BlogDto>(blog);
    }

    public async Task<BlogDto> CreateBlogAsync(CreateBlogDto createBlogDto)
    {
        var blog = _mapper.Map<Blog>(createBlogDto);
        
        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<BlogDto>(blog);
    }

    public async Task<BlogDto> UpdateBlogAsync(int id, UpdateBlogDto updateBlogDto)
    {
        var blog = await _context.Blogs.FindAsync(id);
        
        if (blog == null)
        {
            return null;
        }

        _mapper.Map(updateBlogDto, blog);
        await _context.SaveChangesAsync();
        
        return _mapper.Map<BlogDto>(blog);
    }

    public async Task<bool> DeleteBlogAsync(int id)
    {
        var blog = await _context.Blogs.FindAsync(id);
        
        if (blog == null)
        {
            return false;
        }

        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();
        
        return true;
    }
}
