using AdOut.Planning.Model.Interfaces.Managers;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace AdOut.Planning.WebApi.Auth
{
    public class ResourceAuthorizationHandler<TResource> : AuthorizationHandler<SameUserRequirement, TResource>
    {
        private readonly IUserManager _userManager;
        public ResourceAuthorizationHandler(IUserManager userManager)
        {
            _userManager = userManager;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SameUserRequirement requirement, TResource resource)
        {
            var resourceType = typeof(TResource);
            var resourceUserIdProp = resourceType.GetProperty("UserId");

            if (resourceUserIdProp == null)
            {
                throw new ArgumentException($"Resource={resourceType.Name} don't have UserId property to pass authorization");
            }

            var resourceUserId = (string)resourceUserIdProp.GetValue(resource);
            var requestUserId = _userManager.GetUserId();

            if (resourceUserId == requestUserId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
