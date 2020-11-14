using AdOut.Planning.Model;
using AdOut.Planning.Model.Interfaces.Managers;
using Microsoft.AspNetCore.Http;

namespace AdOut.Planning.Core.Managers
{
    public class UserManager : IUserManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirst(Constants.ClaimsTypes.UserId)?.Value;
        }
    }
}
