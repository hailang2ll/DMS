using DMS.Redis;
using DMS.Sample31.Contracts;
using DMSN.Common.BaseResult;
using DMSN.Common.Extensions;
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
    public class CommonController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IProductService _productservice;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productservice"></param>
        public CommonController(IProductService productservice)
        {
            this._productservice = productservice;
        }

        /// <summary>
        /// accesstoken=dylan 测试IOC，测试token验证，测试属性认证
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("GetShopLogo")]
        public async Task<ResponseResult<UserEntity>> GetShopLogo([FromQuery] UserEntity param)
        {
            //var (loginFlag, result) = await ChenkLoginAsync<UserEntity>();
            //if (!loginFlag)
            //{
            //    return result;
            //}

            var result = await _productservice.GetProductAsync(10);
            //var i = _userService.Add();//未构造，验证属性注入


            //var redisValue = await _redisRepository.GetValue("dylan");
            //var id = redisValue.ID;

            UserEntity user = new UserEntity()
            {
                UserID = 1,
                UserName = "hailang",
                Pwd = "123456",
                Time = DateTime.Now,
            };

            return result;
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
