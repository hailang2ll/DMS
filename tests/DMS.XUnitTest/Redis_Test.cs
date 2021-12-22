using DMS.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DMS.XUnitTest
{
    public class UserTicket
    {
        public string ID { get; set; } = "0";
        public string Name { get; set; }
        public DateTime ExpDate { get; set; } = DateTime.Now.AddDays(90);
        public string Msg { get; set; } = "初始化";
        public int Code { get; set; }

        public string UnionID { get; set; }

    }
    public class Redis_Test : Base_Test
    {
        private RedisManager redisManager = null;
        private RedisManager redisManager2 = null;
        public Redis_Test()
        {
            redisManager = new RedisManager(0);
            redisManager2 = new RedisManager(2);
        }
        /// <summary>
        /// string 操作
        /// </summary>
        [Fact]
        public void StringSet()
        {
            var flag = redisManager.StringSet("dylan", "公众号为：sharecore");
            var value = redisManager.StringGet("dylan");
            Console.WriteLine($"获取缓存值为：{value}");
        }

        /// <summary>
        /// HashTest
        /// </summary>
        [Fact]
        public void HashTest()
        {
            var data = new
            {
                name = "dylan",
                code = "sharecore",
            };
            bool flag = redisManager2.HashExists("dylan", "key1");
            if (!flag)
            {
                redisManager2.HashSet<object>("dylan", "key1", data);
                redisManager2.HashSet<object>("dylan", "key2", data);
            }
            var value = redisManager2.HashValues<QueueValue>("dylan");
            List<QueueValue> list0 = redisManager2.HashValues<QueueValue>("1000_wx7250ec58129a26f1");


        }

        /// <summary>
        /// SortedSetTest
        /// </summary>
        [Fact]
        public void SortedSetTest()
        {

            RedisManager redisManager = new RedisManager(2);
            var data = new SortedSetParam()
            {
                name = "dylan",
                code = "sharecore2",
            };

            var value = redisManager.SortedSetAdd("dylan", data, 1);

            var r = redisManager.SortedSetRangeByRank<SortedSetParam>("dylan");

            //var data = new
            //{
            //    name = "dylan",
            //    code = "sharecore",
            //};
            //redisManager.SortedSetAdd("dylan", data, 1.0);//添加
            //redisManager.SortedSetRemove("dylan",data);//删除
            //redisManager.SortedSetRangeByRank<QueueValue>("dylan");//获取全部
            //redisManager.SortedSetLength("dylan");//获取数量
        }


        /// <summary>
        /// 入队出队/入栈出栈
        /// ListRightLeft
        /// </summary>
        [Fact]
        public void ListRightLeft()
        {
            #region List入队
            redisManager.ListRightPush("dylan", "sharecore我是入队的");//入队
            long len = redisManager.ListLength("dylan");//队列长度
            string b = redisManager.ListRightPop<string>("dylan");//出队
            #endregion


            #region List入栈
            redisManager.ListLeftPush("dylan", "sharecore我是入栈的");//入栈
            len = redisManager.ListLength("dylan");//队列长度
            b = redisManager.ListLeftPop<string>("key123");//出栈
            #endregion
        }

        /// <summary>
        /// 发布订阅
        /// </summary>
        [Fact]
        public void RedisPublish()
        {
            redisManager.Subscribe("dylan", (channel, value) =>
            {
                Console.WriteLine(channel.ToString() + ":" + value.ToString());
            });


            for (int i = 0; i < 50; i++)
            {
                var data = new
                {
                    name = "dylan",
                    code = "sharecore"
                };
                redisManager.Publish("dylan", data);
            };
        }

        /// <summary>
        /// 测试身份缓存
        /// </summary>
        [Fact]
        public void RedisTicket()
        {
            //RedisToken authBase = new RedisToken("1F5A707995391AD7DFE50709A54E264C");
            //UserTicket userTicket = authBase.UserTicket;

            var userTicket = redisManager.StringGet<UserTicket>("dylan");

            //var Ip = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault<IPAddress>(a => a.AddressFamily.ToString().Equals("InterNetwork")).ToString();

        }



        public class queuea
        {
            public string videoKey { get; set; }
            public string Keywords { get; set; }
        }

        public class SortedSetParam
        {
            public string name { get; set; }
            public string code { get; set; }
        }


    }

    public class Queue
    {
        public string key { get; set; }
        public QueueValue Value { get; set; }
    }

    public class QueueValue
    {
        public string WorkQueueType { get; set; }
        public string WorkQueueNo { get; set; }
        public int RelateType { get; set; }
        public string RelateNo { get; set; }
        public string TemplateID { get; set; }
        public string Keywords { get; set; }
        public string Url { get; set; }
        public string ToMemberID { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
