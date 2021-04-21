using Blog.Models;
using Blog.Models.Blog;
using Blog.Models.BlogViewModels;
using Blog.Models.HomeViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.BusinessManagers.Interfaces
{
    public interface IBlogBusinessManager
    {
        Task<BlogModel> CreateBlog(CreateViewModel createBlogViewModel, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal);
        Task<ActionResult<EditViewModel>> UpdateBlog(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal);
        IndexViewModel GetIndexViewModel(string searchString, int? page);
        Task<ActionResult<BlogViewModel>> GetBlogViewModel(int? id, ClaimsPrincipal claimsPrincipal);
    }
}
