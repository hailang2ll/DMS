using DMS.Exceptionless.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using System;

namespace DMS.Exceptionless.Filters
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;
        public GlobalExceptionFilter(IWebHostEnvironment env)
        {
            _env = env;
        }
        public void OnException(ExceptionContext context)
        {
            var json = new DMS.Common.Model.Result.ResponseResult()
            {
                errno = 500,//系统异常代码
                errmsg = "系统异常，请联系客服",//系统异常信息 
            };
            //这里面是自定义的操作记录日志
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                json.errmsg = "用户自定义错误，Message:" + context.Exception.Message;
                if (_env.IsDevelopment())
                {
                    json.errmsg += ",StackTrace:" + context.Exception.StackTrace;
                }
                context.Exception.Submit(json.errmsg);
                context.Result = new BadRequestObjectResult(json);
            }
            else
            {
                json.errmsg = "内部错误，Message:" + context.Exception.Message;
                if (_env.IsDevelopment())
                {
                    json.errmsg += ",StackTrace:" + context.Exception.StackTrace;//堆栈信息
                }
                context.Exception.Submit(json.errmsg);
                context.Result = new InternalServerErrorObjectResult(json);
            }

        }



        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object value) : base(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
    }

    /// <summary>
    /// 操作日志
    /// </summary>
    public class UserOperationException : Exception
    {
        public UserOperationException() { }
        public UserOperationException(string message) : base(message) { }
        public UserOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
