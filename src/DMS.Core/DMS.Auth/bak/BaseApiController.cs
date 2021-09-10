using Microsoft.AspNetCore.Mvc.Filters;

namespace DMS.Auth
{
    /// <summary>
    /// 获取用户信息：不强制退出
    /// </summary>
    public class BaseApiController : BaseController
    {

        //public override void OnActionExecuting(ActionExecutingContext context)
        //{
        //    base.OnActionExecuting(context);
        //    CheckLogin(context, 0);

        //}
    }
}
