using DMS.Common.JsonHandler;
using DMS.Common.Model.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DMS.Auth.Token.FilterAttribute
{
    /// <summary>
    /// 
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
            if (userAuth.ID <= 0)
            {
                ResponseResult result = new ResponseResult()
                {
                    errno = 30,
                    errmsg = "请重新登录.",
                };
                context.Result = new ContentResult() { Content = result.SerializeObject(), StatusCode = 200 };
            }
        }
    }
}
