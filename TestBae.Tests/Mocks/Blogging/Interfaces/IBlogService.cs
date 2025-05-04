using System.Collections.Generic;
using System.Threading.Tasks;
using TestBae.Tests.Mocks.Blogging.DTOs;

namespace TestBae.Tests.Mocks.Blogging.Interfaces;

public interface IBlogService
{
    Task<List<BlogDto>> GetAllBlogsAsync();
    Task<BlogDto> GetBlogByIdAsync(int id);
    Task<BlogDto> CreateBlogAsync(CreateBlogDto createBlogDto);
    Task<BlogDto> UpdateBlogAsync(int id, UpdateBlogDto updateBlogDto);
    Task<bool> DeleteBlogAsync(int id);
}
