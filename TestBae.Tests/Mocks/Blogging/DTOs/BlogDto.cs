using System.Collections.Generic;

namespace TestBae.Tests.Mocks.Blogging.DTOs;

public class BlogDto
{
    public int BlogId { get; set; }
    public string Url { get; set; }
    public int Rating { get; set; }
    public List<PostDto> Posts { get; set; } = new List<PostDto>();
}
