using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Authorizations.UserContext.Token.FilterAttribute
{
    /// <summary>
    /// 全局注册
    /// 在实例化构造函数之后
    /// 做权限控制
    /// 日志
    /// 数据检验
    /// 性能监控
    /// 数据压缩
    /// </summary>
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
}
