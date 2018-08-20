using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using DMS.Exceptionless.Extensions;
using DMS.Exceptionless.Result;
using System;
using System.Net;

namespace DMS.Exceptionless.Filters
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// 当系统发生异常时调用
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            var result = new BaseResult()
            {
                errno = 500,//系统异常代码
                errmsg = "系统异常，请联系客服",//系统异常信息
            };
            Exception ex = context.Exception;
            context.ExceptionHandled = true;
            context.Result = new ObjectResult(result);
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            ex.Submit("系统异常");//提交异常
        }
    }
}