using Blog.BusinessManagers.Interfaces;
using Blog.Models.AdminViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminBusinessManager _adminBusinessManager;

        public AdminController(IAdminBusinessManager adminBusinessManager)
        {
            _adminBusinessManager = adminBusinessManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _adminBusinessManager.GetAdminDashboard(User));
        }

        public async Task<IActionResult> About()
        {
            return View(await _adminBusinessManager.GetAboutViewModel(User));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAbout(AboutViewModel aboutViewModel)
        {
            await _adminBusinessManager.UpdateAbout(aboutViewModel,User);
            return RedirectToAction("Index");
        }

    }
}
