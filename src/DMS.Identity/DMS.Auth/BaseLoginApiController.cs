using Microsoft.AspNetCore.Mvc.Filters;
using DMS.BaseFramework.Common.Enums;

namespace DMS.Auth
{
    /// <summary>
    /// 获取用户信息：检查登录状态，未登录直接退出
    /// </summary>
    public class BaseLoginApiController : BaseController
    {

        /// <summary>
        /// 用户票据
        /// </summary>
        //public TicketEntity CurrentUserTicket
        //{
        //    get;
        //    private set;
        //}

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CheckLogin(context, (int)EnumCheckLogin.CheckLogin_Exit);


            //Microsoft.Extensions.Primitives.StringValues token = context.HttpContext.Request.Headers["AccessToken"];
            //if (!string.IsNullOrWhiteSpace(token))
            //{
            //    //存在AccessToken值，进行验证
            //    RedisCacheTicket authBase = new RedisCacheTicket(token);
            //    TicketEntity userTicket = authBase.CurrentUserTicket;
            //    if (userTicket != null && userTicket.MemberID > 0)
            //    {
            //        CurrentUserTicket = userTicket;
            //        return;
            //    }
            //}

            ////直接输出结果，不经过Controller
            //ResponseResult result = new ResponseResult()
            //{
            //    errno = 30,
            //    errmsg = "请重新登录",
            //};
            //context.Result = new ContentResult() { Content = SerializerJson.SerializeObject(result), StatusCode = 200 };

        }
    }
}
