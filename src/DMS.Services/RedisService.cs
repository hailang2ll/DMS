using DMS.Auth;
using DMS.Common.JsonHandler;
using DMS.IServices;
using DMS.Redis;
using DMS.Services.RedisEvBus;

namespace DMS.Services
{
    public class RedisService : IRedisService
    {
        private readonly IRedisRepository redisRepository;
        public RedisService(IRedisRepository redisRepository)
        {
            this.redisRepository = redisRepository;
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
                UID = "unid",
                ExpDate = DateTime.Now.AddDays(1)
            };
            //我是写入
            var setAsync = await redisRepository.SetAsync("string", "我是内容", DateTime.Now.AddDays(1));
            setAsync = await redisRepository.SetAsync("string1", "我是内容1", DateTime.Now.AddDays(1) - DateTime.Now);
            setAsync = await redisRepository.SetAsync("json", userTicket.SerializeObject(), DateTime.Now.AddDays(1) - DateTime.Now);

            //我是获取
            var getValueAsync = await redisRepository.GetValueAsync("string");
            getValueAsync = await redisRepository.GetValueAsync("string1");
            var json = await redisRepository.GetValueAsync<UserTicket>("json");

            //我是判断是否存在
            var existAsync = await redisRepository.ExistAsync("string");
            existAsync = await redisRepository.ExistAsync("string1");
            existAsync = await redisRepository.ExistAsync("json");

            //我是删除
            var removeAsync = await redisRepository.RemoveAsync("string");
            removeAsync = await redisRepository.RemoveAsync("string1");
            removeAsync = await redisRepository.RemoveAsync("json");

            //我在0库
            setAsync = await redisRepository.SetAsync("string0", msg, DateTime.Now.AddDays(1));

            #region HashExistsAsync
            /*
             user_1:
             ID:1,Key:vis_1,Value:{"ID":1,"Name":"dylan","ExpDate":"2021-09-08 17:22:54","Msg":"我是消息","Code":2,"UnionID":"unid"}
             ID:2,Key:vis_2,Value:{"ID":2,"Name":"dylan","ExpDate":"2021-09-08 17:22:54","Msg":"我是消息","Code":2,"UnionID":"unid"}
             ID:3,Key:vis_3,Value:{"ID":3,"Name":"dylan","ExpDate":"2021-09-08 17:22:54","Msg":"我是消息","Code":2,"UnionID":"unid"}
             */
            //我是hash值存储
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
                UID = "unid",
                ExpDate = DateTime.Now.AddDays(1)
            };

            redisRepository.ChangeDatabase(1);
            var setAsync = await redisRepository.SetAsync("string1", msg, DateTime.Now.AddDays(1));
        }


        /// <summary>
        /// 发布
        /// </summary>
        void IRedisService.Publish()
        {
            UserTicket userTicket = new UserTicket()
            {
                ID = 1,
                Name = "dylan",
                UID = "unid",
                ExpDate = DateTime.Now.AddDays(1)
            };
            redisRepository.PublishAsync("queueName", userTicket);
        }
        /// <summary>
        /// 订阅
        /// </summary>
        void IRedisService.Subscribe()
        {
            redisRepository.SubscribeAsync("queueName", (channel, message) =>
            {
                Console.WriteLine($"接收消息：message={message}");
            });
        }


        /// <summary>
        /// 订阅服务先注入
        /// 发布
        /// </summary>
        /// <returns></returns>
        public async Task RedisPublish()
        {
            var msg = $"这里是一条日志{DateTime.Now}";
            await redisRepository.ListLeftPushAsync(RedisUtil.QueueLoging, msg);
        }
    }
}
