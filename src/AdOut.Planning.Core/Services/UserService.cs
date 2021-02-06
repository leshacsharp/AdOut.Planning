using AdOut.Extensions.Authorization;
using AdOut.Planning.Model.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace AdOut.Planning.Core.Managers
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypeNames.UserId)?.Value;
        }
    }
}
