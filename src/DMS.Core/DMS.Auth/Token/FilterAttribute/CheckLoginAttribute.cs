using DMS.Common.JsonHandler;
using DMS.Common.Model.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net;

namespace DMS.Auth.Token.FilterAttribute
{
    /// <summary>
    /// 第一个管道
    /// 全局过虑：option.Filters.Add<AuthorizationFilter>();
    /// 控制器过虑：[AuthorizationFilter]
    /// 如果存在构造函数调用：[TypeFilter(typeof(AuthorizationFilter))]
    /// 不同实现思路
    /// </summary>
    public class AuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isDefined = false;
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                //true跳出
                isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                  .Any(a => a.GetType().Equals(typeof(JumpCheckLoginAttribute)));
            }
            if (!isDefined)
            {
                var userAuthService = context.HttpContext.RequestServices.GetRequiredService<IUserAuth>();
                if (userAuthService.ID <= 0)
                {
                    ResponseResult result = new ResponseResult(HttpStatusCode.Unauthorized);
                    context.Result = new ContentResult() { Content = result.SerializeObject(), StatusCode = (int)HttpStatusCode.OK };
                }
            }
        }
    }

    /// <summary>
    /// 统一登录验证
    /// 控制器调用:[TypeFilter(typeof(CheckLoginAttribute))]
    /// ation跳过认证：[JumpCheckLogin]
    /// 不同实现思路
    /// </summary>
    public class CheckLoginAttribute : ActionFilterAttribute
    {
        private readonly IUserAuth userAuth;
        public CheckLoginAttribute(IUserAuth userAuth)
        {
            this.userAuth = userAuth;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var isDefined = false;
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                //true跳出
                isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                  .Any(a => a.GetType().Equals(typeof(JumpCheckLoginAttribute)));
            }
            if (!isDefined)
            {
                if (userAuth.ID <= 0)
                {
                    ResponseResult result = new ResponseResult(HttpStatusCode.Unauthorized);
                    context.Result = new ContentResult() { Content = result.SerializeObject(), StatusCode = (int)HttpStatusCode.OK };
                }
            }
        }
    }
}
