using DMS.Auth;
using DMS.Sample31.Contracts;
using DMSN.Common.BaseResult;
using DMSN.Common.Extensions;
using DMSN.Common.Helper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Sample31.Controllers
{
    /// <summary>
    /// Common api 测试
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        public IUserService _userService { get; set; }
        /// <summary>
        /// 
        /// </summary>

        public IProductService _productservice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productservice"></param>
        public CommonController(IProductService productservice)
        {
            _productservice = productservice;
        }

        /// <summary>
        /// 测试IOC，测试token验证，测试属性认证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("GetShopLogo")]
        public Task<ResponseResult<UserEntity>> GetShopLogo([FromQuery]UserEntity param)
        {
            var (loginFlag, result) = ChenkLogin<UserEntity>(UserTicket);
            if (!loginFlag)
            {
                return Task.FromResult(result);
            }

            var b = _productservice.Add();
            //var i = _userService.Add();//未构造，验证属性注入

            UserEntity user = new UserEntity()
            {
                UserID = 1,
                UserName = "hailang",
                Pwd = "123456",
                Time = DateTime.Now,
            };
            result.data = user;
            return Task.FromResult(result);
        }

        #region GetLog4net
        /// <summary>
        /// 日志处理
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetLog4net")]
        public ActionResult GetLog4net()
        {
            DMS.Log4net.Logger.Info("这是log4net的日志");
            DMS.Log4net.Logger.Error("这是log4net的异常日志");

            DMS.NLogs.Logger.Debug("这是nlog的Debug日志");
            DMS.NLogs.Logger.Info("这是nlog的日志");
            DMS.NLogs.Logger.Error("这是nlog的异常日志");
            var result = new
            {
                data = "成功"
            };
            return Ok(result);
        }
        #endregion

        /// <summary>
        /// 读取appsettings.json和自定义配置文件
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAppConfig")]
        public ActionResult GetAppConfig()
        {
            string memberApi = DMSN.Common.AppConfig.GetVaule<string>("MemberUrl");
            memberApi = DMSN.Common.AppConfig.GetVaule("MemberUrl");
            var ip = $"获取IP：{IPHelper.GetWebClientIp()}";
            var dev = DMSN.Common.AppConfig.GetVaule("dev");
            var redisOption = DMS.Redis.AppConfig.RedisOption;


            var result = new
            {
                memberApi,
                ip,
                dev,
                redisOption
            };
            return Ok(result);
        }

       

        /// <summary>
        /// StringConvertAll
        /// </summary>
        /// <returns></returns>
        [HttpGet("StringConvertAll")]
        public ActionResult StringConvertAll()
        {
            //List转字符串
            List<string> List = new List<string>();
            string strArray = string.Join(",", List);

            //字符串转List
            string str = "2,4,4,4";
            List = new List<string>(str.Split(','));

            //字符数组转Int数组
            int[] list = Array.ConvertAll<string, int>(str.Split(','), s => int.Parse(s));
            long[] cartIds = Array.ConvertAll<string, long>(str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries), q => q.ToLong());
            string[] arr = str.Split(new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries);

            //List<string>字符串转Int数组
            List = new List<string>();
            strArray = string.Join(",", List);
            list = Array.ConvertAll<string, int>(strArray.Split(','), s => int.Parse(s));

            List<Guid?> ids = List.ConvertAll<Guid?>(q => { return q.ToGuid(); });
            //Guid?[] strategyKeys = Array.ConvertAll<string, Guid?>(param.ToArray(), item => TryParse.StrToGuid(item));
            //Array.ConvertAll<string, Guid?>(StrategyKeys.ToArray(), item => { return TryParse.StrToGuid(item); });
            
            return Ok();
        }
    }
}
