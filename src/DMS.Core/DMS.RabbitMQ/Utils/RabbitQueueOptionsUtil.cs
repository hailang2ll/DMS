using DMS.RabbitMQ.Context;
using DMS.RabbitMQ.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DMS.RabbitMQ.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class RabbitQueueOptionsUtil
    {


        /// <summary>
        /// 根据对列名称获取对列模型
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public static RabbitMessageQueueOptions ReaderByQueue(string queueName)
        {
            List<RabbitMessageQueueOptions> messageQueueOptionsList = ReaderAll();
            RabbitMessageQueueOptions entity = messageQueueOptionsList.Where(q => q.QueueName == queueName).FirstOrDefault();
            return entity;
        }

        /// <summary>
        /// 加载所有对列配置文件
        /// </summary>
        /// <returns></returns>
        private static List<RabbitMessageQueueOptions> ReaderAll()
        {
            List<RabbitMessageQueueOptions> messageQueueOptionsList = new List<RabbitMessageQueueOptions>();
            List<ExchangeConfigOptions> exchangeConfigOptionsList = RabbitMQContext.Config.ExchangeConfig;
            if (exchangeConfigOptionsList == null || exchangeConfigOptionsList.Count <= 0)
            {
                throw new ArgumentNullException("ReaderAll:加载配置文件为空");
            }
            foreach (ExchangeConfigOptions configOptions in exchangeConfigOptionsList)
            {
                foreach (var queueItem in configOptions.Queues)
                {
                    RabbitMessageQueueOptions queueOptions = new RabbitMessageQueueOptions()
                    {
                        ExchangeName = configOptions.ExchangeName,
                        ExchangeType = configOptions.ExchangeType,
                        Persistent = configOptions.Persistent,
                        QueueName = queueItem.QueueName,
                        RoutingKeys = queueItem.RoutingKeys,
                    };
                    messageQueueOptionsList.Add(queueOptions);
                }
            }
            return messageQueueOptionsList;
        }
    }
}
