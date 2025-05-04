using System.Collections.Generic;
using System.Threading.Tasks;
using TestBae.Tests.Mocks.Blogging.DTOs;

namespace TestBae.Tests.Mocks.Blogging.Interfaces;

public interface IPostService
{
    Task<List<PostDto>> GetAllPostsAsync();
    Task<List<PostDto>> GetPostsByBlogIdAsync(int blogId);
    Task<PostDto> GetPostByIdAsync(int id);
    Task<PostDto> CreatePostAsync(CreatePostDto createPostDto);
    Task<PostDto> UpdatePostAsync(int id, UpdatePostDto updatePostDto);
    Task<bool> DeletePostAsync(int id);
}
