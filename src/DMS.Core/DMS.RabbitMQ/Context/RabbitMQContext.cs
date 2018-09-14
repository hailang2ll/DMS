using DMS.BaseFramework.Common.Configuration;
using DMS.RabbitMQ.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DMS.RabbitMQ.Context
{
    class RabbitMQContext
    {
        //RabbitMQ建议客户端线程之间不要共用Model，至少要保证共用Model的线程发送消息必须是串行的，但是建议尽量共用Connection。
        private static readonly ConcurrentDictionary<string, IModel> ModelDic = new ConcurrentDictionary<string, IModel>();
        /// <summary>
        /// 配置文件
        /// </summary>
        public static RabbitOptions Config;
        /// <summary>
        /// Socket链接
        /// </summary>
        public static IConnection Connection;
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
            Config = AppSettings.GetCustomValue<RabbitOptions>(fileDir, fileName);
            if (Config == null)
            {
                string errMsg = $"配置文件：{configPath}初始化异常！！！";
                throw new Exception(errMsg);
            }

            //创建链接工厂
            var connectionStrings = Config.ConnectionString;
            ConnectionFactory ConnectionFactory = new ConnectionFactory()
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
        /// 
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static IModel GetModel(string queue)
        {
            return ModelDic.GetOrAdd(queue, value =>
            {
                var model = Connection.CreateModel();
                ModelDic[queue] = model;
                return model;
            });
        }

    }
}
