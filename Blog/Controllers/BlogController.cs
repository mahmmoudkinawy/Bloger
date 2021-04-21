using Blog.BusinessManagers.Interfaces;
using Blog.Models.Blog;
using Blog.Models.BlogViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {
        private readonly IBlogBusinessManager _blogBusinessManager;

        public BlogController(IBlogBusinessManager blogBusinessManager)
        {
            _blogBusinessManager = blogBusinessManager;
        }

        [AllowAnonymous]
        [Route("Post/{id}")]
        public async Task<IActionResult> Index(int? id)
        {
            var actionResult = await _blogBusinessManager.GetBlogViewModel(id, User);

            if (actionResult.Result is null)
                return View(actionResult.Value);

            return actionResult.Result;
        }

        public IActionResult Create()
        {
            return View(new CreateViewModel());
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var actionResult = await _blogBusinessManager.GetEditViewModel(id, User);

            if (actionResult.Result is null)
                return View(actionResult.Value);

            return actionResult.Result;
        }


        [HttpPost]
        public async Task<IActionResult> Add(CreateViewModel createViewModel)
        {
            await _blogBusinessManager.CreateBlog(createViewModel, User);
            return RedirectToAction("Index", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> Update(EditViewModel editViewModel)
        {
            var actionResult = await _blogBusinessManager.UpdateBlog(editViewModel, User);

            if (actionResult.Result is null)
                return RedirectToAction("Edit", new { editViewModel.Blog.Id });

            return actionResult.Result;
        }
    }
}
