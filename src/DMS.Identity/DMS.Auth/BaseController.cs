using DMS.Auth.Tickets;
using DMS.Common.BaseResult;
using DMS.Common.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DMS.Auth
{
    /// <summary>
    /// 验证登录的情况
    /// </summary>
    public class BaseController : Controller
    {
        /// <summary>
        /// 当前会话Id
        /// </summary>
        public string Sid
        {
            get
            {
                string sId = string.Empty;
                if (Request.Headers.ContainsKey("AccessToken"))
                {
                    sId = Request.Headers["AccessToken"];
                }
                return sId;
            }
        }

        /// <summary>
        /// 用户票据
        /// </summary>
        public TicketEntity CurrentUserTicket { get; private set; }

        /// <summary>
        /// 验证登录的情况
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type">0=检查登录，获取用户信息，不退出；1=检查登录，未登录直接退出</param>
        protected void CheckLogin(ActionExecutingContext context, int type)
        {
            var controllerName = context.RouteData.Values["Controller"].ToString();
            var actionName = context.RouteData.Values["Action"].ToString();
            CurrentUserTicket = new TicketEntity();
            Microsoft.Extensions.Primitives.StringValues token = context.HttpContext.Request.Headers["AccessToken"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                //存在AccessToken值，进行验证，以后升级方法
                RedisCacheTicket authBase = new RedisCacheTicket(token);
                TicketEntity userTicket = authBase.CurrentUserTicket;
                if (userTicket != null && userTicket.ID.ToLong() > 0)
                {
                    CurrentUserTicket = userTicket;
                    return;
                }
                else
                {
                    System.Console.WriteLine($"获取缓存身份信息：{userTicket.Msg}，{controllerName}/{actionName}");
                }
            }


            if (type == 1)
            {
                //以上检查未登录，直接退出
                //直接输出结果，不经过Controller
                ResponseResult result = new ResponseResult()
                {
                    errno = 30,
                    errmsg = "身份过期，请重新登录",
                };
                context.Result = new ContentResult() { Content = result.SerializeObject(), StatusCode = 200 };
            }
        }
    }
}