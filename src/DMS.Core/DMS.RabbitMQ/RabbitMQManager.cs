using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DMS.BaseFramework.Common.Configuration;
using DMS.Exceptionless;

namespace DMS.RabbitMQ
{
    public class RabbitMQManager
    {

        public static RabbitMQConfig rabbitConfig { get; set; }

        static RabbitMQManager()
        {
            rabbitConfig = AppSettings.GetCustomModel<RabbitMQConfig>("RabbitMQ.json");
        }
        /// <summary>
        /// 创建IConnection
        /// </summary>
        /// <returns></returns>
        static IConnection CreateConnection
        {
            get
            {
                if (rabbitConfig != null)
                {
                    ConnectionFactory connFactory = new ConnectionFactory()
                    {
                        HostName = rabbitConfig.HostName,
                        UserName = rabbitConfig.UserName,
                        Password = rabbitConfig.Password,
                        VirtualHost = rabbitConfig.VirtualHost,
                        Port = rabbitConfig.Port,
                        RequestedHeartbeat = rabbitConfig.RequestedHeartbeat,//心跳超时时间
                        AutomaticRecoveryEnabled = rabbitConfig.AutomaticRecoveryEnabled,//自动重新连接
                    };
                    return connFactory.CreateConnection("jlh");//创建连接
                }
                else
                {
                    throw new Exception("没有找到RabbitMQ.config的配置文件");
                }
            }
        }

        /// <summary>
        /// 生产者（发送）
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="message"></param>
        /// <returns></returns> 
        public bool Send<T>(string queueName, T message) where T : class
        {
            using (IConnection connection = CreateConnection)
            {
                using (IModel channel = connection.CreateModel())
                {
                    //设置交换器的类型
                    string ExchangeName = "Exchange_" + queueName;
                    channel.ExchangeDeclare(ExchangeName, "direct", true, false, null);

                    bool durable = true;//不会丢失队列，消息持久机制,持久化准备
                    channel.QueueDeclare(queueName, durable, false, false, null);

                    //绑定消息队列，交换器，routingkey
                    channel.QueueBind(queueName, ExchangeName, "Key_" + queueName);

                    //消息持久化语句
                    IBasicProperties properties = channel.CreateBasicProperties();
                    properties.DeliveryMode = 2;
                    properties.Persistent = true;
                    byte[] bytes;
                    bytes = SerializeObject<T>(message);
                    channel.BasicPublish("", queueName, properties, bytes);
                }
            }
            LessLog.Info("发送成功，服务器IP为：" + rabbitConfig.HostName);
            return true;
        }

        public void Receive<T>(string queueName, Action<T> action) where T : class
        {
            IConnection connection = CreateConnection;

            IModel channel = connection.CreateModel();

            //设置交换器的类型
            string ExchangeName = "Exchange_" + queueName;
            channel.ExchangeDeclare(ExchangeName, "direct", true, false, null);

            bool durable = true;
            //在MQ上定义一个队列，如果名称相同不会重复创建
            channel.QueueDeclare(queueName, durable, false, false, null);

            channel.QueueBind(queueName, ExchangeName, "Key_" + queueName);

            //公平分发
            channel.BasicQos(0, 1, false);


            //在队列上定义一个消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
            channel.BasicConsume(queueName, false, consumer);
            consumer.Received += (o, e) =>
            {
                var tcs = new TaskCompletionSource<object>();
                byte[] bytes = e.Body;
                T entity = DeserializeObject<T>(bytes);
                try
                {
                    action(entity);
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
                // 如果 channel.BasicConsume 中参数 noAck 设置为 false，必须加上消息确认语句
                // Message acknowledgment（消息确认机制作用）
                // consumer dies(its channel is closed, connection is closed, or TCP connection is lost)
                channel.BasicAck(e.DeliveryTag, false);
                Thread.Sleep(1000);
            };



        }

        /// <summary>
        /// 获取对列中的数量
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public uint GetMessageCount(string queueName)
        {
            using (IConnection connection = CreateConnection)
            {
                using (IModel channel = connection.CreateModel())
                {
                    uint count = channel.MessageCount(queueName);
                    return count;
                }
            }
        }
        /// <summary>
        /// 序列化成字节流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        private byte[] SerializeObject<T>(T entity)
        {
            XmlSerializer xs = new XmlSerializer(entity.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                xs.Serialize(ms, entity);
                byte[] bytes = ms.ToArray();
                return bytes;
            }
        }

        /// <summary>
        /// 反序列化成实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private T DeserializeObject<T>(byte[] data)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream(data))
            {
                T entity = (T)xs.Deserialize(ms);
                return entity;
            }

            //string json = Encoding.UTF8.GetString(data);
            //T entity = JsonHelper.DeserializeObject<T>(json);
            //return entity;
        }

    }
}
