using Blog.Models;
using System.Threading.Tasks;

namespace Blog.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> Update(ApplicationUser applicationUser);
        ApplicationUser Get(string id);
    }
}
