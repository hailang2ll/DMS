using InitQ.Abstractions;
using InitQ.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Services.RedisEvBus
{
    public class RedisSubscribe : IRedisSubscribe
    {

        [Subscribe(RedisUtil.QueueLoging)]
        private async Task SubRedisLoging(string msg)
        {
            Console.WriteLine($"订阅者 1 从 队列{RedisUtil.QueueLoging} 消费到/接受到 消息:{msg}");

            await Task.CompletedTask;
        }
    }
}
