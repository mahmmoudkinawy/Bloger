using Blog.BusinessManagers.Interfaces;
using Blog.Models;
using Blog.Models.HomeViewModels;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PagedList.Core;
using System.Linq;

namespace Blog.BusinessManagers
{
    public class HomeBusinessManager : IHomeBusinessManager
    {
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;

        public HomeBusinessManager(IBlogService blogService,IUserService userService)
        {
            _blogService = blogService;
            _userService = userService;
        }

        public ActionResult<AuthorViewModel> GetAuthorViewModel(string authorId,string searchString , int? page)
        {
            if (authorId is null)
                return new BadRequestResult();

            var applicationUser = _userService.Get(authorId);

            if (applicationUser is null)
                return new NotFoundResult();

            int pageSize = 20;
            int pageNumber = page ?? 1;

            var blogs = _blogService.GetBlogs(searchString ?? string.Empty)
                .Where(blog => blog.Published && blog.Creator == applicationUser);

            return new AuthorViewModel
            {
                Author = applicationUser,
                Blogs = new StaticPagedList<BlogModel>(blogs.Skip((pageNumber - 1) * pageSize).Take(pageSize), pageNumber, pageSize, blogs.Count()),
                SearchString = searchString,
                PageNumber = pageNumber
            };

        }

    }
}
