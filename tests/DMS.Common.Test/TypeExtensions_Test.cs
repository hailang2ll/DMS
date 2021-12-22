using DMS.Common.Extensions;
using NUnit.Framework;
using System;

namespace DMS.Common.Test
{
    /// <summary>
    /// 
    /// </summary>
    public class TypeExtensions_Test
    {
        [Test]
        public void Test1()
        {
            Type type = typeof(MessageModel);
            var a = type.GetAttribute<RabbitMqAttribute>(true);
            Assert.Pass();
        }

    }

    [RabbitMq("Test.QueueName", ExchangeName = "Test.ExchangeName", IsProperties = false)]
    public class MessageModel
    {
        public string Msg { get; set; }

        public DateTime CreateDateTime { get; set; }
    }

    /// <summary>
    /// Demo自定义的RabbitMq队列信息实体特性
    /// </summary>
    public class RabbitMqAttribute : Attribute
    {
        public RabbitMqAttribute(string queueName)
        {
            QueueName = queueName ?? string.Empty;
        }

        /// <summary>
        /// 交换机名称
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; private set; }

        /// <summary>
        /// 是否持久化
        /// </summary>
        public bool IsProperties { get; set; }
    }




}
