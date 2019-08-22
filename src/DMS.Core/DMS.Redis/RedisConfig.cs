using DMS.Redis.Configurations;
using StackExchange.Redis;
using System;
using System.Collections.Concurrent;

namespace DMS.Redis
{
    public class RedisConfig
    {
        //系统自定义Key前缀
        public static string SysCustomKey = "";
        //"127.0.0.1:6379,allowadmin=true
        private static string RedisConnectionString = "192.168.0.167:6340,allowadmin=true";
        private static string RedisPwd = "redis123";

        private static readonly ConcurrentDictionary<string, ConnectionMultiplexer> ConnectionCache = new ConcurrentDictionary<string, ConnectionMultiplexer>();
        private static readonly object Locker = new object();
        private static ConnectionMultiplexer _instance;

        static RedisConfig()
        {
            RedisOption option = AppConfig.RedisOption;
            if (option != null)
            {
                SysCustomKey = option.RedisPrefixKey;
                RedisConnectionString = option.RedisConnectionString;
                RedisPwd = option.RedisConnectionPwd;
            }
            else
            {
                throw new Exception($"未加载redis.json文件");
            }
        }

        /// <summary>
        /// 单例获取
        /// </summary>
        public static ConnectionMultiplexer Instance
        {
            get
            {

                if (_instance == null)
                {
                    lock (Locker)
                    {
                        if (_instance == null || !_instance.IsConnected)
                        {
                            try
                            {
                                _instance = GetManager();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("初始化Redis缓存错误," + ex.Message);
                            }
                        }
                    }
                }
                return _instance;

            }
        }

        /// <summary>
        /// 缓存获取
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static ConnectionMultiplexer GetConnectionMultiplexer(string connectionString)
        {
            if (!ConnectionCache.ContainsKey(connectionString))
            {
                ConnectionCache[connectionString] = GetManager(connectionString);
            }
            return ConnectionCache[connectionString];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static ConnectionMultiplexer GetManager(string connectionString = null)
        {
            connectionString = connectionString ?? RedisConnectionString;

            var options = ConfigurationOptions.Parse(connectionString);
            //options.ClientName = "root";
            options.AbortOnConnectFail = false;
            options.Password = RedisPwd;
            var connect = ConnectionMultiplexer.Connect(options);

            //注册如下事件
            connect.ConnectionFailed += MuxerConnectionFailed;
            connect.ConnectionRestored += MuxerConnectionRestored;
            connect.ErrorMessage += MuxerErrorMessage;
            connect.ConfigurationChanged += MuxerConfigurationChanged;
            connect.HashSlotMoved += MuxerHashSlotMoved;
            connect.InternalError += MuxerInternalError;
            return connect;
        }


        #region 事件
        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConfigurationChanged(object sender, EndPointEventArgs e)
        {
            System.Console.WriteLine("Configuration changed: " + e.EndPoint);
            //LessLog.Info("Configuration changed: " + e.EndPoint);
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerErrorMessage(object sender, RedisErrorEventArgs e)
        {
            System.Console.WriteLine("ErrorMessage: " + e.Message);
            //LessLog.Info("ErrorMessage: " + e.Message);
        }

        /// <summary>
        /// 重新建立连接之前的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            System.Console.WriteLine("ConnectionRestored: " + e.EndPoint);
            //LessLog.Info("ConnectionRestored: " + e.EndPoint);
        }

        /// <summary>
        /// 连接失败 ， 如果重新连接成功你将不会收到这个通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            System.Console.WriteLine("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
            //LessLog.Info("重新连接：Endpoint failed: " + e.EndPoint + ", " + e.FailureType + (e.Exception == null ? "" : (", " + e.Exception.Message)));
        }

        /// <summary>
        /// 更改集群
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerHashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            System.Console.WriteLine("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
            //LessLog.Info("HashSlotMoved:NewEndPoint" + e.NewEndPoint + ", OldEndPoint" + e.OldEndPoint);
        }

        /// <summary>
        /// redis类库错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void MuxerInternalError(object sender, InternalErrorEventArgs e)
        {
            System.Console.WriteLine("InternalError:Message" + e.Exception.Message);
            //LessLog.Info("InternalError:Message" + e.Exception.Message);
        }

        #endregion 事件


    }
}
