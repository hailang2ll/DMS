using DMS.Common.Model.Result;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace DMS.Extensions.Filter
{
    /// <summary>
    /// 入参统一验证
    /// </summary>
    public class ApiResultFilterAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (!context.ModelState.IsValid)
            {
                //初始化返回结果
                var result = new ResponseResult();
                result.errno = 1;
                result.errmsg = string.Join(Environment.NewLine, context.ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)));
                //foreach (var item in context.ModelState.Values)
                //{
                //    foreach (var error in item.Errors)
                //    {
                //        result.errmsg += error.ErrorMessage + "|";
                //    }
                //}
                context.Result = new JsonResult(result);
            }
        }
    }
}
