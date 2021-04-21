using Blog.Authorization;
using Blog.BusinessManagers.Interfaces;
using Blog.Models;
using Blog.Models.Blog;
using Blog.Models.BlogViewModels;
using Blog.Models.HomeViewModels;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.BusinessManagers
{
    public class BlogBusinessManager : IBlogBusinessManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBlogService _blogService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAuthorizationService _authorizationService;

        public BlogBusinessManager(UserManager<ApplicationUser> userManager,
                                  IBlogService blogService,
                                  IWebHostEnvironment webHostEnvironment,
                                  IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _blogService = blogService;
            _webHostEnvironment = webHostEnvironment;
            _authorizationService = authorizationService;
        }

        public IndexViewModel GetIndexViewModel(string searchString, int? page)
        {
            int pageSize = 20;
            int pageNumber = page ?? 1;
            var blogs = _blogService.GetBlogs(searchString ?? string.Empty)
                .Where(blog => blog.Published);

            return new IndexViewModel
            {
                Blogs = new StaticPagedList<BlogModel>(blogs.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, blogs.Count()),
                SearchString = searchString,
                PageNumber = pageNumber
            };
        }

        public async Task<ActionResult<BlogViewModel>> GetBlogViewModel(int? id, ClaimsPrincipal claimsPrincipal)
        {
            if (id is null)
                return new BadRequestResult();

            var blogId = id.Value;

            var blog = _blogService.GetBlog(blogId);

            if (blog is null)
                return new NotFoundResult();

            if (!blog.Published)
            {
                var authorizationResult = await _authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Read);

                if (!authorizationResult.Succeeded)
                    return DetermineActionResult(claimsPrincipal);
            }

            return new BlogViewModel
            {
                Blog = blog
            };
        }


        public async Task<BlogModel> CreateBlog(CreateViewModel createViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var blog = createViewModel.Blog;

            blog.Creator = await _userManager.GetUserAsync(claimsPrincipal);
            blog.CreatedOn = DateTime.Now;
            blog.UpdatedOn = DateTime.Now;

            blog = await _blogService.Add(blog);

            string webRootPath = _webHostEnvironment.WebRootPath;
            string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";

            EnsureFolder(pathToImage);

            using (var fileStream = new FileStream(pathToImage, FileMode.Create))
            {
                await createViewModel.BlogHeaderImage.CopyToAsync(fileStream);
            }

            return blog;
        }

        public async Task<ActionResult<EditViewModel>> UpdateBlog(EditViewModel editViewModel, ClaimsPrincipal claimsPrincipal)
        {
            var blog = _blogService.GetBlog(editViewModel.Blog.Id);

            if (blog is null)
                return new NotFoundResult();

            var authorizationResult = await _authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);

            if (!authorizationResult.Succeeded)
                return DetermineActionResult(claimsPrincipal);

            blog.Published = editViewModel.Blog.Published;
            blog.Title = editViewModel.Blog.Title;
            blog.Content = editViewModel.Blog.Content;
            blog.UpdatedOn = DateTime.Now;

            if (editViewModel.BlogHeaderImage != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Blogs\{blog.Id}\HeaderImage.jpg";

                EnsureFolder(pathToImage);

                using (var fileStream = new FileStream(pathToImage, FileMode.Create))
                {
                    await editViewModel.BlogHeaderImage.CopyToAsync(fileStream);
                }
            }

            return new EditViewModel
            {
                Blog = await _blogService.Update(blog)
            };

        }


        public async Task<ActionResult<EditViewModel>> GetEditViewModel(int? id, ClaimsPrincipal claimsPrincipal)
        {
            if (id is null)
                return new BadRequestResult();

            var blogId = id.Value;

            var blog = _blogService.GetBlog(blogId);

            if (blog is null)
                return new NotFoundResult();

            var authorizationResult = await _authorizationService.AuthorizeAsync(claimsPrincipal, blog, Operations.Update);

            if (!authorizationResult.Succeeded)
                return DetermineActionResult(claimsPrincipal);

            return new EditViewModel
            {
                Blog = blog
            };

        }

        private ActionResult DetermineActionResult(ClaimsPrincipal claimsPrincipal)
        {
            if (claimsPrincipal.Identity.IsAuthenticated)
                return new ForbidResult();
            else
                return new ChallengeResult();
        }


        private void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName.Length > 0)
                Directory.CreateDirectory(Path.GetDirectoryName(path));
        }

    }
}
