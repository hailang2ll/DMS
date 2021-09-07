using DMS.Auth.Tickets;
using DMS.Redis;
using DMS.Sample31.Contracts;
using DMSN.Common.BaseResult;
using DMSN.Common.JsonHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Sample31.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IRedisRepository redisRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="redisRepository"></param>
        public ValuesController(IProductService productService, IRedisRepository redisRepository)
        {
            this.productService = productService;
            this.redisRepository = redisRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("SetRedis")]
        public async Task SetRedis()
        {
            UserTicket userTicket = new UserTicket()
            {
                ID = 1,
                Name = "dylan",
                Code = 2,
                Msg = "我是消息",
                UnionID = "unid",
                ExpDate = DateTime.Now.AddDays(1)
            };
            var setAsync = await redisRepository.SetAsync("string", "我是内容", DateTime.Now.AddDays(1));
            setAsync = await redisRepository.SetAsync("string1", "我是内容1", DateTime.Now.AddDays(1) - DateTime.Now);
            setAsync = await redisRepository.SetAsync("json", userTicket.SerializeObject(), DateTime.Now.AddDays(1) - DateTime.Now);

            var getValueAsync = await redisRepository.GetValueAsync("string");
            getValueAsync = await redisRepository.GetValueAsync("string1");
            var json = await redisRepository.GetValueAsync<UserTicket>("json");

            var existAsync = await redisRepository.ExistAsync("string");
            existAsync = await redisRepository.ExistAsync("string1");
            existAsync = await redisRepository.ExistAsync("json");

            var removeAsync = await redisRepository.RemoveAsync("string");
            removeAsync = await redisRepository.RemoveAsync("string1");
            removeAsync = await redisRepository.RemoveAsync("json");


            #region HashExistsAsync
            /*
             user_1:
             ID:1,Key:vis_1,Value:{"ID":1,"Name":"dylan","ExpDate":"2021-09-08 17:22:54","Msg":"我是消息","Code":2,"UnionID":"unid"}
             ID:2,Key:vis_2,Value:{"ID":2,"Name":"dylan","ExpDate":"2021-09-08 17:22:54","Msg":"我是消息","Code":2,"UnionID":"unid"}
             ID:3,Key:vis_3,Value:{"ID":3,"Name":"dylan","ExpDate":"2021-09-08 17:22:54","Msg":"我是消息","Code":2,"UnionID":"unid"}
             */
            for (int i = 0; i < 10; i++)
            {
                string visid = "vis_" + i;
                bool flag = await redisRepository.HashExistsAsync("user_1", visid);
                if (!flag)
                    flag = await redisRepository.HashSetAsync("user_1", visid, userTicket);

                flag = await redisRepository.HashDeleteAsync("user_1", visid);
            }
            #endregion


            #region List
            var listLength = await redisRepository.ListLengthAsync("list");
            if (listLength < 10)
            {
                //获取指KEY的集合
                List<UserTicket> list = await redisRepository.ListRangeAsync<UserTicket>("list");

                //入栈
                listLength = await redisRepository.ListLeftPushAsync<string>("list", "aaaaaa");
                //出栈
                var str = await redisRepository.ListLeftPopAsync<string>("list");

                //入队
                listLength = await redisRepository.ListRightPushAsync<string>("list", "bbbb");
                //出队
                str = await redisRepository.ListRightPopAsync<string>("list");
            }
            #endregion




        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        public async Task<ResponseResult<UserEntity>> GetProductAsync(long productID)
        {
            var userEntity = await redisRepository.GetValueAsync<UserTicket>("dylan");
            return await productService.GetProductAsync(productID);
        }

    }
}
