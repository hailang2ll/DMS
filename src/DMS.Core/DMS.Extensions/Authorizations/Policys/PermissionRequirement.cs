#if NET5_0 || NETCOREAPP3_1
using Microsoft.AspNetCore.Authorization;

namespace DMS.Extensions.Authorizations.Policys
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string _permissionName { get; }

        public PermissionRequirement(string PermissionName)
        {
            _permissionName = PermissionName;
        }
    }
}
#endif
