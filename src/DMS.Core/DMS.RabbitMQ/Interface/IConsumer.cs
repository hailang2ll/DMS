using DMS.RabbitMQ.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.RabbitMQ.Interface
{
    /// <summary>
    /// 消费者接口
    /// </summary>
    public interface IConsumer
    {
        /// <summary>
        /// 队列处理方法
        /// </summary>
        ResponseMQResult Handler(ConsumerContext context);
    }
}
