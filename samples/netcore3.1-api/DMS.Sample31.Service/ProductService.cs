using DMS.Auth;
using DMS.Redis;
using DMS.Sample31.Contracts;
using DMS.Sample31.Service.RedisEvBus;
using DMSN.Common.BaseResult;
using DMSN.Common.JsonHandler;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Sample31.Service
{
    public class ProductService : IProductService
    {
        private readonly IRedisRepository redisRepository;
        public ProductService(IRedisRepository redisRepository)
        {
            this.redisRepository = redisRepository;
        }
        public async Task<ResponseResult<UserEntity>> GetProductAsync(long jobLogID)
        {
            ResponseResult<UserEntity> result = new ResponseResult<UserEntity>();
            UserEntity entity = new UserEntity()
            {
                UserID = 1125964271981826048,
                UserName = "aaaa",
                Pwd = "pwd"
            };
            result.data = entity;
            return await Task.FromResult(result);

        }
        /// <summary>
        /// 我应该在0库
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task TestRedis0(string msg = "我应该在0库")
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


            setAsync = await redisRepository.SetAsync("string0", msg, DateTime.Now.AddDays(1));

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
                listLength = await redisRepository.ListLeftPushAsync<UserTicket>("list", userTicket);
                //出栈
                var str = await redisRepository.ListLeftPopAsync<UserTicket>("list");

                //入队
                listLength = await redisRepository.ListRightPushAsync<string>("list", "aaaaaaaaaaa");
                //出队
                var str2 = await redisRepository.ListRightPopAsync<string>("list");
            }
            #endregion
        }
        /// <summary>
        /// 我应该在1库
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task TestRedis1(string msg = "我应该在1库")
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

            redisRepository.ChangeDatabase(1);
            var setAsync = await redisRepository.SetAsync("string1", msg, DateTime.Now.AddDays(1));
        }

        public async Task RedisPublish()
        {
            var msg = $"这里是一条日志{DateTime.Now}";
            await redisRepository.ListLeftPushAsync(RedisUtil.QueueLoging, msg);
        }


        void IProductService.Publish()
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
            redisRepository.PublishAsync("queueName", userTicket);
        }

        void IProductService.Subscribe()
        {
            redisRepository.SubscribeAsync("queueName", (channel, message) =>
            {
                Console.WriteLine($"message={message},数据序列化错误");
            });
        }
    }
}
