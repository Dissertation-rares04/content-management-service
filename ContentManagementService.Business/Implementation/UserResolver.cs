using ContentManagementService.Business.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ContentManagementService.Business.Implementation
{
    public class UserResolver : IUserResolver
    {
        private readonly string _userId;

        public UserResolver(IHttpContextAccessor httpContextAccessor)
        {
            _userId = (httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value) ?? throw new Exception("No sub claim found");
        }

        public string UserId => _userId;
    }
}
