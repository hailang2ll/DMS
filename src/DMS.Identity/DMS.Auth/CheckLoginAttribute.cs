using DMS.Auth.Tickets;
using DMS.Redis;
using DMSN.Common.BaseResult;
using DMSN.Common.Extensions;
using DMSN.Common.JsonHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace DMS.Auth
{

    /// <summary>
    /// 第一个管道
    /// 同步，全局过虑
    /// option.Filters.Add<AuthorizationFilter>();
    /// </summary>
    public class AuthorizationFilter : IAuthorizationFilter
    {
        /// <summary>
        /// 用户票据
        /// </summary>
        public UserTicket CurrentUserTicket
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
                //RedisCacheTicket authBase = new RedisCacheTicket(token);
                //TicketEntity userTicket = authBase.CurrentUserTicket;
                //if (userTicket != null && userTicket.ID.ToLong() > 0)
                //{
                //    CurrentUserTicket = userTicket;
                //    return;
                //}
            }

            //直接输出结果，不经过Controller
            ResponseResult result = new ResponseResult()
            {
                errno = 30,
                errmsg = "请重新登录",
            };
            context.Result = new ContentResult() { Content = result.SerializeObject(), StatusCode = 200 };
        }
    }
    /// <summary>
    ///  异步
    ///  检查登录，授权验证
    ///  [HttpGet("GetShopLogo"), LoginFilter("123")]
    ///  如果全局注入，Attribute与构造函数去掉
    /// </summary>
    public class LoginFilter : Attribute, IAsyncAuthorizationFilter
    {
        public LoginFilter()
        {
        }

        public LoginFilter(string funId)
        {
            FunId = funId;
        }

        /// <summary>
        /// 方法标识id
        /// </summary>
        private string FunId { get; set; }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool isAjax = IsAjax(context.HttpContext.Request);
            string sessionJson = context.HttpContext.Session.GetString("user");
            if (string.IsNullOrWhiteSpace(sessionJson))
            {
                if (isAjax)
                {
                    //无权访问
                    //context.Result = new UnauthorizedResult();
                    //context.Result = new JsonResult(new { Code = 500, Msg = "登录失效，请重新登录" })
                    //{
                    //    StatusCode = StatusCodes.Status401Unauthorized
                    //};
                    //return Task.CompletedTask;
                }
                //没有登录，去登录
                context.Result = new RedirectResult("/user/LoginView");
                //return Task.CompletedTask;
            }
            //if (string.IsNullOrWhiteSpace(FunId))
            //{
            //    //return Task.CompletedTask;
            //}

            //var userView = System.Text.Json.JsonSerializer.Deserialize<UserView>(sessionJson);
            //bool have = userView.HaveMenuList.Any(x => x.Id == FunId);
            //if (have)
            //{
            //    return Task.CompletedTask;
            //}
            //if (isAjax)
            //{
            //    //无权访问
            //    //context.Result = new UnauthorizedResult();
            //    context.Result = new JsonResult(new { Code = 500, Msg = "你无权访问" })
            //    {
            //        StatusCode = StatusCodes.Status401Unauthorized
            //    };
            //}
            //else
            //{
            //    //无权访问
            //    //context.Result = new UnauthorizedResult();
            //    context.Result = new ContentResult()
            //    {
            //        Content = "你无权访问",
            //        StatusCode = StatusCodes.Status401Unauthorized
            //    };
            //}
            return Task.CompletedTask;
        }

        /// <summary>
        /// 判断是否为ajax请求
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public static bool IsAjax(HttpRequest req)
        {
            //X-Requested-With: XMLHttpRequest
            bool result = false;
            var xreq = req.Headers.ContainsKey("x-requested-with");
            if (xreq)
            {
                result = req.Headers["x-requested-with"] == "XMLHttpRequest";
            }
            return result;
        }
    }
    public class GlobalActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //Loggger.Info("OnActionExecuting");
            //执行方法前先执行这
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            System.Diagnostics.Debug.WriteLine("执行方法后执行这");
            //Loggger.Info("OnActionExecuted");
            //执行方法后执行这
        }
    }


    /// <summary>
    /// 属性检查登录
    /// [HttpGet("GetShopLogo"), CheckLogin]
    /// </summary>
    public class CheckLoginAttribute : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// 用户票据
        /// </summary>
        public UserTicket UserTicket
        {
            get;
            private set;
        }

        private RedisManager redisManager { get; set; }
        private RedisManager RedisManager
        {
            get
            {
                if (redisManager == null)
                {
                    redisManager = new RedisManager(0);
                }
                return redisManager;
            }
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            Microsoft.Extensions.Primitives.StringValues token = context.HttpContext.Request.Headers["AccessToken"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                //存在AccessToken值，进行验证
                var userTicket = RedisManager.StringGet<UserTicket>(token);
                if (userTicket != null && userTicket.ID.ToLong() > 0)
                {
                    UserTicket = userTicket;
                    return;
                }
            }

            //其它情况直接跳出，直接输出结果
            ResponseResult result = new ResponseResult()
            {
                errno = 30,
                errmsg = "请重新登录",
            };
            context.Result = new ContentResult() { Content = result.SerializeObject(), StatusCode = 200 };
        }
    }



}
