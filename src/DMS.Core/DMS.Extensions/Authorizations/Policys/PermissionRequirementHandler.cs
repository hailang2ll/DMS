#if NET5_0 || NETCOREAPP3_1
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.Extensions.Authorizations.Policys
{
    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IHttpContextAccessor _accessor;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="schemes"></param>
        /// <param name="roleModulePermissionServices"></param>
        /// <param name="accessor"></param>
        public PermissionRequirementHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var httpContext = _accessor.HttpContext;
            var role = context.User.FindFirst(c => c.Type == ClaimTypes.Role);
            if (role != null)
            {
                var roleValue = role.Value;
                if (roleValue == requirement._permissionName)
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
#endif