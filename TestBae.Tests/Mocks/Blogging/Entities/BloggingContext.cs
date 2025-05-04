using Microsoft.EntityFrameworkCore;

namespace TestBae.Tests.Mocks.Blogging.Entities;

public class BloggingContext : DbContext
{
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    
    public BloggingContext(DbContextOptions<BloggingContext> options)
        : base(options)
    {
    }
}