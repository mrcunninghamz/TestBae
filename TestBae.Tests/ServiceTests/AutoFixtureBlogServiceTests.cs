using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestBae.BaseClasses.AutoFixture.EntityFramework;
using TestBae.Tests.Mocks.Blogging.DTOs;
using TestBae.Tests.Mocks.Blogging.Entities;
using TestBae.Tests.Mocks.Blogging.Services;
using Xunit;

namespace TestBae.Tests.ServiceTests;

public class AutoFixtureBlogServiceTests : BaseTest<BlogService, BloggingContext>
{
    public AutoFixtureBlogServiceTests()
    {
        // Sample data for tests
        var blogs = new List<Blog>
        {
            new Blog { BlogId = 1, Url = "https://blog1.com", Rating = 4, Posts = new List<Post>() },
            new Blog { BlogId = 2, Url = "https://blog2.com", Rating = 5, Posts = new List<Post>() },
            new Blog { BlogId = 3, Url = "https://blog3.com", Rating = 3, Posts = new List<Post>() }
        };

        var posts = new List<Post>
        {
            new Post { PostId = 1, Title = "Post 1", Content = "Content 1", BlogId = 1 },
            new Post { PostId = 2, Title = "Post 2", Content = "Content 2", BlogId = 1 },
            new Post { PostId = 3, Title = "Post 3", Content = "Content 3", BlogId = 2 }
        };

        // Link posts to blogs
        blogs[0].Posts.Add(posts[0]);
        blogs[0].Posts.Add(posts[1]);
        blogs[1].Posts.Add(posts[2]);
        
        DbContext.Blogs.AddRange(blogs);
        DbContext.SaveChanges();
    }
    
    [Fact]
    public async Task GetAllBlogsAsync_ShouldReturnAllBlogs()
    {
        // Arrange
        // Act
        var result = await TestSubject.GetAllBlogsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("https://blog1.com", result[0].Url);
        Assert.Equal(5, result[1].Rating);
    }

    [Fact]
    public async Task GetBlogByIdAsync_WithValidId_ShouldReturnBlog()
    {
        // Arrange
        var blogId = 1;
        
        // Act
        var result = await TestSubject.GetBlogByIdAsync(blogId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(blogId, result.BlogId);
        Assert.Equal("https://blog1.com", result.Url);
        Assert.Equal(4, result.Rating);
        Assert.Equal(2, result.Posts.Count);
    }

    [Fact]
    public async Task GetBlogByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var blogId = 99;

        // Act
        var result = await TestSubject.GetBlogByIdAsync(blogId);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateBlogAsync_ShouldCreateAndReturnBlog()
    {
        // Arrange
        var createBlogDto = new CreateBlogDto
        {
            Url = "https://newblog.com",
            Rating = 5
        };

        // Act
        var result = await TestSubject.CreateBlogAsync(createBlogDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(createBlogDto.Url, result.Url);
        Assert.Equal(createBlogDto.Rating, result.Rating);
        var dbBlog = await DbContext.Blogs.SingleOrDefaultAsync(x => x.Url.Equals(createBlogDto.Url));
        Assert.NotNull(dbBlog);
    }

}