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
        public const string AuditQueueName = "dms.rabbitmq.auditqueue";
        /// <summary>
        /// 任务交换机
        /// 备注：任务交换机用来做，消息定时发送、消息重试
        /// </summary>
        public const string TaskExchangeName = "dms.rabbitmq.direct.task";
        /// <summary>
        /// 配置文件初始化
        /// </summary>
        /// <param name="fileDir"></param>
        /// <param name="fileName"></param>
        public static void Configure(string fileDir, string fileName)
        {
            var configPath = Path.Combine(fileDir, fileName);
            if (!File.Exists(configPath))
            {
                string errMsg = $"配置文件：{configPath}不存在！！！";
                throw new FileNotFoundException(errMsg);
            }

            //加载配置文件
            Config = AppSettings.GetCustomValue<RabbitmqOptions>(fileDir, fileName);
            if (Config == null)
            {
                string errMsg = $"配置文件：{configPath}初始化异常！！！";
                throw new Exception(errMsg);
            }

            //创建链接工厂
            var connectionStrings = Config.ConnectionString;
            ConnectionFactory = new ConnectionFactory()
            {
                HostName = connectionStrings.Host,
                Port = connectionStrings.Port,
                UserName = connectionStrings.UserName,
                Password = connectionStrings.Password,
                //自动重新连接
                AutomaticRecoveryEnabled = true, 
                //心跳超时时间
                RequestedHeartbeat = connectionStrings.TimeOut
            };
            //创建链接
            Connection = ConnectionFactory.CreateConnection();
        }

        /// <summary>
        /// 创建链接
        /// </summary>
        //public static IModel GetModel()
        //{
        //    try
        //    {
        //        if (!Connection.IsOpen)
        //        {
        //            //创建链接
        //            Connection = ConnectionFactory.CreateConnection();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        //创建链接
        //        Connection = ConnectionFactory.CreateConnection();
        //    }
        //    return Connection.CreateModel();
        //}
    }
}
