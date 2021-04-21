using Blog.BusinessManagers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogBusinessManager _blogBusinessManager;
        private readonly IHomeBusinessManager _homeBusinessManager;

        public HomeController(IBlogBusinessManager blogBusinessManager,IHomeBusinessManager homeBusinessManager)
        {
            _blogBusinessManager = blogBusinessManager;
            _homeBusinessManager = homeBusinessManager;
        }

        public IActionResult Index(string searchString,int? page)
        {
            return View(_blogBusinessManager.GetIndexViewModel(searchString, page));
        }

        public IActionResult Author(string authorId,string searchString,int? page)
        {
            var actionResult = _homeBusinessManager.GetAuthorViewModel(authorId, searchString, page);
            if (actionResult.Result is null)
                return View(actionResult.Value);

            return actionResult.Result;
        }


    }
}
