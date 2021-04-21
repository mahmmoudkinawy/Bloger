using Blog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Services.Interfaces
{
    public interface IBlogService
    {
        Task<BlogModel> Add(BlogModel blog);
        IEnumerable<BlogModel> GetBlogs(ApplicationUser applicationUser);
        BlogModel GetBlog(int blogId);
        Task<BlogModel> Update(BlogModel blog);
        IEnumerable<BlogModel> GetBlogs(string searchString);
    }
}
