using AdOut.Planning.Model;
using System.Linq;
using System.Security.Claims;

namespace AdOut.Planning.WebApi.Auth
{
    public static class ClaimsHelper
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return principal.Claims.SingleOrDefault(c => c.Type == Constants.ClaimsTypes.UserId)?.Value;
        }
    }
}
