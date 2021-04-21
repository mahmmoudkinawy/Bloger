using Blog.Data;
using Blog.Models;
using Blog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class BlogService : IBlogService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public BlogService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public BlogModel GetBlog(int blogId)
        {
            return _applicationDbContext.Blogs
                .Include(blog => blog.Creator)
                .Include(blog => blog.Posts)
                    .ThenInclude(comment => comment.Poser)
                .Include(blog => blog.Posts)
                    .ThenInclude(comment => comment.Poser)
                .FirstOrDefault(blog => blog.Id == blogId);
        }

        public IEnumerable<BlogModel> GetBlogs(string searchString)
        {
            return _applicationDbContext.Blogs
                .OrderByDescending(blog => blog.UpdatedOn)
                .Include(blog => blog.Creator)
                .Include(blog => blog.Posts)
                .Where(blog => blog.Title.Contains(searchString) || blog.Content.Contains(searchString));
        }

        public IEnumerable<BlogModel> GetBlogs(ApplicationUser applicationUser)
        {
            return _applicationDbContext.Blogs
                .Include(blog => blog.Creator)
                .Include(blog => blog.Approver)
                .Include(blog => blog.Posts)
                .Where(blog => blog.Creator == applicationUser);
        }




        public async Task<BlogModel> Add(BlogModel blog)
        {
            _applicationDbContext.Add(blog);
            await _applicationDbContext.SaveChangesAsync();
            return blog;
        }

        public async Task<BlogModel> Update(BlogModel blog)
        {
            _applicationDbContext.Update(blog);
            await _applicationDbContext.SaveChangesAsync();
            return blog;
        }
    }
}
