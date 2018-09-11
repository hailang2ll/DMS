using DMS.Redis;
using System;

namespace DMS.CommonTest
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisTest();
        }

        /// <summary>
        /// 测试缓存
        /// </summary>
        static void RedisTest()
        {
            RedisManager redisManager = new RedisManager(0);
            var all = redisManager.GetAll();
            foreach (var item in all)
            {
                Console.WriteLine($"集合缓存，key：{item.Key}，value：{item.Value}");
            }

            var flag = redisManager.StringSet("key", "dylan");
            Console.WriteLine($"缓存成功，状态值为：{flag}");
            var value = redisManager.StringGet("key");
            Console.WriteLine($"获取缓存值为：{value}");

            flag = redisManager.KeyDelete("key");
            Console.WriteLine($"删除缓存，状态值为：{flag}");


        }
    }
}
 