using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DMS.Auth.Tickets;
using DMS.BaseFramework.Common.BaseResult;
using DMS.BaseFramework.Common.Serializer;

namespace DMS.Auth
{

    /// <summary>
    /// 第一个管道
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        /// <summary>
        /// 用户票据
        /// </summary>
        public TicketEntity CurrentUserTicket
        {
            get;
            private set;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Microsoft.Extensions.Primitives.StringValues token = context.HttpContext.Request.Headers["AccessToken"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                //存在AccessToken值，进行验证
                RedisCacheTicket authBase = new RedisCacheTicket(token);
                TicketEntity userTicket = authBase.CurrentUserTicket;
                if (userTicket != null && userTicket.MemberID > 0)
                {
                    CurrentUserTicket = userTicket;
                    return;
                }
            }

            //直接输出结果，不经过Controller
            ResponseResult result = new ResponseResult()
            {
                errno = 30,
                errmsg = "请重新登录",
            };
            context.Result = new ContentResult() { Content = SerializerJson.SerializeObject(result), StatusCode = 200 };
        }
    }


    /// <summary>
    /// 属性检查登录
    /// </summary>
    public class CheckLoginAttribute : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// 用户票据
        /// </summary>
        public TicketEntity CurrentUserTicket
        {
            get;
            private set;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            Microsoft.Extensions.Primitives.StringValues token = context.HttpContext.Request.Headers["AccessToken"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                //存在AccessToken值，进行验证
                RedisCacheTicket authBase = new RedisCacheTicket(token);
                TicketEntity userTicket = authBase.CurrentUserTicket;
                if (userTicket != null && userTicket.MemberID > 0)
                {
                    CurrentUserTicket = userTicket;
                    return;
                }
            }

            //直接输出结果，不经过Controller
            ResponseResult result = new ResponseResult()
            {
                errno = 30,
                errmsg = "请重新登录",
            };
            context.Result = new ContentResult() { Content = SerializerJson.SerializeObject(result), StatusCode = 200 };
        }
    }
}
