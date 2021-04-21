using Blog.Data;
using Blog.Models;
using Blog.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UserService(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public ApplicationUser Get(string id)
        {
            return _applicationDbContext.Users
                .FirstOrDefault(user => user.Id == id);
        }

        public async Task<ApplicationUser> Update(ApplicationUser applicationUser)
        {
            _applicationDbContext.Update(applicationUser);
            await _applicationDbContext.SaveChangesAsync();
            return applicationUser;
        }

    }
}
