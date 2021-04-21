using Blog.BusinessManagers.Interfaces;
using Blog.Models;
using Blog.Models.AdminViewModels;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.BusinessManagers
{
    public class AdminBusinessManager : IAdminBusinessManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminBusinessManager(UserManager<ApplicationUser> userManager,
                IBlogService blogService,
                IUserService userService,
                IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _blogService = blogService;
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IndexViewModel> GetAdminDashboard(ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await _userManager.GetUserAsync(claimsPrincipal);
            return new IndexViewModel
            {
                Blogs = _blogService.GetBlogs(applicationUser)
            };
        }

        public async Task<AboutViewModel> GetAboutViewModel(ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await _userManager.GetUserAsync(claimsPrincipal);
            return new AboutViewModel
            {
                ApplicationUser = applicationUser,
                SubHeader = applicationUser.SubHeader,
                Content = applicationUser.AboutContent
            };
        }

        public async Task UpdateAbout(AboutViewModel aboutViewModel,ClaimsPrincipal claimsPrincipal)
        {
            var applicationUser = await _userManager.GetUserAsync(claimsPrincipal);

            applicationUser.SubHeader = aboutViewModel.SubHeader;
            applicationUser.AboutContent = aboutViewModel.Content;

            if (aboutViewModel.HeaderImage != null)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string pathToImage = $@"{webRootPath}\UserFiles\Users\{applicationUser.Id}\HeaderImage.jpg";

                EnsureFolder(pathToImage);

                using (var fileStream = new FileStream(pathToImage, FileMode.Create))
                {
                    await aboutViewModel.HeaderImage.CopyToAsync(fileStream);
                }
            }

            await _userService.Update(applicationUser);

        }

        private void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            if (directoryName.Length > 0)
                Directory.CreateDirectory(Path.GetDirectoryName(path));
        }


    }
}
