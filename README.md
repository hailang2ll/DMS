## DMS

### DMS将会是一个集中式的中间件框架，每一个小型中间件将会是完全独立的，如：gRPC，Thrift，netty，Wcf，Exceptionless，Ocelot，RabbitMQ，Redis，IdentityServer，Consul，Zookeeper等，Demo中会有每一个中间件的实例方便开发者了解 



## DMS.Exceptionless 基于Exceptionless框架开发，依赖DMS中基础框架，主要用于分页式日志系统
github源码地址：https://github.com/exceptionless/Exceptionless.Net
参考资料将客户端服务配置起来，目前仅支持ElasticSearch5.X版本

### 项目调用方法
```c# 
LessLog.Info("这是一条提示信息");
LessLog.Error("这是一条错误的信息");
LessLog.Fatal("这是一条致命的信息");
```


## DMS.Autofac 基于Autofac框架，完全自动注入，默认都是以构造函数来注入，在这里支持属性注入，简单代码方便调用修复了之前版本的注入问题
在Startup中的方法添加：return AutofacService.RegisterAutofac(services, "项目接口名.Contracts", "项目服务名.Service");即可属性注入，方法返回类型为：IServiceProvider

### 项目调用方法
```c# 
public IDemoService service { get; set; } 只需在调用的位置定义接口属性就好
var entity1 = service.GetEntity(13); 在方法中直接调用，不需要在实例化
```


## DMS.RabbitMQ 基于RabbitMQ.Client框架开发，依赖DMS中基础框架，主要用于分布式消息对列系统
首先定义你的数据实体文件RabbitMQ.json
```c# 
public class RabbitMQConfig
{

        /// <summary>
        /// MQ服务器
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 虚拟主机
        /// </summary>
        public string VirtualHost { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 心跳数
        /// </summary>
        public ushort RequestedHeartbeat { get; set; }

        /// <summary>
        /// 自动重新连接
        /// </summary>
        public bool AutomaticRecoveryEnabled { get; set; }

        /// <summary>
        ///是否发送
        /// </summary>
        public bool IsSend { get; set; }
}
```
### 项目调用方法
```c# 
RabbitMQHelper help = new RabbitMQHelper();
help.Send<NoticeMessageParam>("Notice", notice);
```



## DMS.Redis 基于StackExchange.Redis框架开发，依赖DMS中基础框架，主要用于分布式缓存系统
首先定义你的数据实体文件Redis.json
```c# 
public class RedisEntityConfig
    {
        #region 属性

        /// <summary>
        /// Redis服务器
        /// </summary>
        public string RedisConnectionString { get; set; }

        /// <summary>
        /// Redis密码
        /// </summary>
        public string RedisConnectionPwd { get; set; }

        /// <summary>
        /// 系统自定义Key前缀
        /// </summary>
        public string RedisPrefixKey { get; set; }

        #endregion
    }
```
### 项目调用方法
```c# 
RedisManager redisManager = new RedisManager(0);
redisManager.StringSet("key", "value1");
var key = redisManager.StringGet("key");
```

### 待完善