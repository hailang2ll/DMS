using DMS.Redis;
using DMSN.Common.BaseResult;
using DMSN.Common.Extensions;
using DMSN.Common.JsonHandler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DMS.Auth.FilterAttribute
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckLoginAttribute : ActionFilterAttribute
    {
        private readonly IRedisRepository redisRepository;

        public CheckLoginAttribute(IRedisRepository redisRepository)
        {
            this.redisRepository = redisRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            Microsoft.Extensions.Primitives.StringValues token = context.HttpContext.Request.Headers["AccessToken"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                //存在AccessToken值，进行验证
                var userTicket = redisRepository.GetValueAsync<UserTicket>(token).Result;
                if (userTicket != null && userTicket.ID.ToLong() > 0)
                {
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
