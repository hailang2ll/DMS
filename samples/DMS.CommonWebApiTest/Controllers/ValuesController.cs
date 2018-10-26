using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Auth;
using DMS.Log4net;
using DMS.NLogs;
using Microsoft.AspNetCore.Mvc;

namespace DMS.CommonWebApiTest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : BaseApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            string sid = this.Sid;
            try
            {
                Logger.Debug($"Log4net调试日志：{Guid.NewGuid().ToString("N")}");
                Logger.Info($"Log4net消息日志：{Guid.NewGuid().ToString("N")}");
                Logger.Warn($"Log4net警告日志：{Guid.NewGuid().ToString("N")}");

                NLogger.Debug($"NLog调试日志：{Guid.NewGuid().ToString("N")}");
                NLogger.Info($"NLog消息日志：{Guid.NewGuid().ToString("N")}");
                NLogger.Warn($"NLog警告日志：{Guid.NewGuid().ToString("N")}");
                throw new NullReferenceException("空异常");
            }
            catch (Exception ex)
            {
                Logger.Error($"Log4net异常日志：{Guid.NewGuid().ToString("N")}", ex);
                NLogger.Error($"NLog异常日志：{Guid.NewGuid().ToString("N")}", ex);
            }


            return new string[] { "value1", "value2" };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
