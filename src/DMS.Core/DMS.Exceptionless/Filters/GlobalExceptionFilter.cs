using DMS.Common.BaseResult;
using DMS.Common.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace DMS.Exceptionless.Filters
{
    /// <summary>
    /// 全局异常处理
    /// </summary>
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        public GlobalExceptionFilter(IHostingEnvironment env)
        {
            _env = env;
        }
        public void OnException(ExceptionContext context)
        {
            var json = new DataResultBase()
            {
                errno = 500,//系统异常代码
                errmsg = "系统异常，请联系客服",//系统异常信息
            };

            /// <summary>
            /// 当系统发生异常时调用，旧版本
            /// </summary>
            /// <param name="context"></param>
            //public void OnException(ExceptionContext context)
            //{
            //    var result = new DataResultBase()
            //    {
            //        errno = 500,//系统异常代码
            //        errmsg = "系统异常，请联系客服",//系统异常信息
            //    };
            //    Exception ex = context.Exception;
            //    context.ExceptionHandled = true;
            //    context.Result = new ObjectResult(result);
            //    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            //    ex.Submit("系统异常");//提交异常
            //}



            //这里面是自定义的操作记录日志
            if (context.Exception.GetType() == typeof(UserOperationException))
            {
                json.errmsg = context.Exception.Message;
            }
            else
            {
                json.errmsg = "发生了未知内部错误";
            }

            if (_env.IsDevelopment())
            {
                json.errmsg = "Development：" + json.errmsg;//堆栈信息
            }
            else
            {
                json.errmsg = "Production：" + json.errmsg;//堆栈信息
            }
            context.Result = new ContentResult() { Content = json.SerializeObject(), StatusCode = 200 };


            //context.Exception.Submit(json.errmsg);

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
