using Microsoft.AspNetCore.Authorization;

namespace ContentManagementService.API.Authorization
{
    class RbacRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public RbacRequirement(string permission)
        {
            Permission = permission ?? throw new ArgumentNullException(nameof(permission));
        }
    }
}
