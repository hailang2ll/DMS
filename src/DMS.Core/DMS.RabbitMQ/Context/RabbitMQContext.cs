using DMS.BaseFramework.Common.Configuration;
using DMS.RabbitMQ.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DMS.RabbitMQ.Context
{
    class RabbitMQContext
    {
        /// <summary>
        /// 私有构造函数，禁止外部使用new关键字创建对象
        /// </summary>
        private RabbitMQContext() { }
        /// <summary>
        /// 配置文件
        /// </summary>
        public static RabbitmqOptions Config;
        /// <summary>
        /// Socket链接
        /// </summary>
        public static IConnection Connection;
        /// <summary>
        /// 链接工厂
        /// </summary>
        public static ConnectionFactory ConnectionFactory;
        /// <summary>
        /// 第几次重试变量名称
        /// </summary>
        public const string RetryCountKeyName = "RetryCount";
        /// <summary>
        /// 审计队列名称
        /// </summary>
        public const string AuditQueueName = "stack.rabbitmq.auditqueue";
        /// <summary>
        /// 任务交换机
        /// 备注：任务交换机用来做，消息定时发送、消息重试
        /// </summary>
        public const string TaskExchangeName = "stack.rabbitmq.direct.task";
        /// <summary>
        /// 配置文件初始化
        /// </summary>
        /// <param name="configPath">文件路径</param>
        public static void Configure(string configPath = "")
        {
            if (!File.Exists(configPath))
            {
                string errMsg = $"配置文件：{configPath}不存在！！！";
                throw new FileNotFoundException(errMsg);
            }

            //加载配置文件
            Config = null;//AppSettings.GetCustomModel<RabbitmqOptions>(fileDir, fileName);
            if (Config == null)
            {
                string errMsg = $"配置文件：{configPath}初始化异常！！！";
                throw new TypeInitializationException("RabbitmqConfig", null);
            }

            //创建链接工厂
            var connectionStrings = Config.ConnectionString;
            ConnectionFactory = new ConnectionFactory()
            {
                Port = connectionStrings.Port,
                AutomaticRecoveryEnabled = true,
                HostName = connectionStrings.Host,
                Password = connectionStrings.Password,
                UserName = connectionStrings.UserName,
                RequestedHeartbeat = connectionStrings.TimeOut
            };
            //创建链接
            Connection = ConnectionFactory.CreateConnection();
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        public static IModel GetModel()
        {
            try
            {
                if (!Connection.IsOpen)
                {
                    //创建链接
                    Connection = ConnectionFactory.CreateConnection();
                }
            }
            catch (Exception)
            {
                //创建链接
                Connection = ConnectionFactory.CreateConnection();
            }
            return Connection.CreateModel();
        }
    }
}
