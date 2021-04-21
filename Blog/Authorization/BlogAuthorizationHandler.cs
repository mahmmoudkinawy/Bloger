
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Blog.Authorization
{
    public class BlogAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, BlogModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public BlogAuthorizationHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, OperationAuthorizationRequirement requirement, BlogModel resource)
        {
            var applicationUser = await _userManager.GetUserAsync(context.User);
            
            if ((requirement.Name == Operations.Update.Name || requirement.Name == Operations.Delete.Name) && applicationUser == resource.Creator)
                context.Succeed(requirement);

            if (requirement.Name == Operations.Read.Name && !resource.Published && applicationUser == resource.Creator)
                context.Succeed(requirement);

        }
    }
}
