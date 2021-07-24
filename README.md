# DMS

### DMS是一个集中式的中间件框架，每一个小型中间件将会是完全独立的，都将以注册方式使用，主要为了快速开发项目而准备，如：DMS.Autofac，DMS.Consul，DMS.RabbitMQg，DMS.Swagger，GRPC，Thrift，Netty，DMS.Exceptionless，DMS.Log4net，DMS.NLogs，DMS.Consul，Ocelot，IdentityServer，Zookeeper等，Demo中会有每一个中间件的实例方便开发者了解 
qq交流群：18362376
<br />
作者微信：hailang2ll

![运营公众号](https://github.com/hailang2ll/DMS.EntityFrameworkCore/blob/master/gzh02.jpg)
<br />

## DMS.Autofac 依赖与注入
基于Autofac框架，支持多种方式注入（构造函数注入，属性注入）

### 1.示例调用
需要先引用Nuget包，安装DMS.Autofac
在Startup类的ConfigureServices方法中添加：

```c# 
第一种：默认查找当前接口类与实现类，规则如：DMS.EntityFrameworkCore.Contracts，DMS.EntityFrameworkCore.Service，后缀.Contracts,.Service结尾，排除不必要的注册类，加快注册速度
第二种：实现类继承统一接口IAutoInject，规则如：实现类DMS.EntityFrameworkCore.Service下的都需要继承IAutoInject，后缀.Service结尾，排除不必要的注册类，加快注册速度
第三种：全局扫描以Service结尾的类，自动注入
第四种：模块化注入，自定义模块实现注入，如：AutofacModule
public IServiceProvider ConfigureServices(IServiceCollection services)
{

    services.AddMvc().AddJsonOptions(options =>
    {
        options.SerializerSettings.DateFormatString = "yyyy-MM-dd";
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    });
    services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    services.AddSwaggerGenV2();

    #region AddDbContext
    var connectionStrings = Configuration.GetConnectionString("trydou_sys");
    services.AddDbContext<trydou_sysContext>(options => options.UseSqlServer(connectionStrings));
    #endregion

    return AutofacService.RegisterAutofac(services);
}

```

## DMS.RabbitMQ 基于RabbitMQ.Client框架开发，依赖DMS中基础框架，主要用于分布式消息对列系统
首先定义你的数据实体文件RabbitMQ.json，参考开源中的JSON文件
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
### 服务端调用方法
```c# 
 static IHost CreateDefaultHost(string[] args) => new HostBuilder()
//.UseLog4net("Config\\log4net.config")
.UseRabbitMQ("Config\\rabbitmq.json")
.UseBusinessHost()//启用业务主机
.UseAuditHost()//启用审计队列
.Build();
```

### 客户端调用方法
```c# 
static IHost CreateDefaultHost(string[] args) => new HostBuilder()
//.UseLog4net("Config\\log4net.config")
.UseRabbitMQ("Config\\rabbitmq.json")
.ConfigureServices((hostContext, services) =>
{
    services.AddHostedService<TestHostService>(); //后用后台服务
})
.Build();
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
### string类型调用方法
```c# 
var flag = redisManager.StringSet("dylan", "公众号为：sharecore");
var value = redisManager.StringGet("dylan");
Console.WriteLine($"获取缓存值为：{value}");
```

### List类型调用方法
入队
```c# 
redisManager.ListRightPush("dylan", "sharecore我是入队的");//入队
long len = redisManager.ListLength("dylan");//队列长度
string b = redisManager.ListRightPop<string>("dylan");//出队
```

出队
```c# 
redisManager.ListLeftPush("dylan", "sharecore我是入栈的");//入栈
long len = redisManager.ListLength("dylan");//队列长度
string b = redisManager.ListLeftPop<string>("key123");//出栈
```

### Hash类型调用方法
```c# 
var data = new
{
    name = "dylan",
    code = "sharecore",
};
bool flag = redisManager.HashExists("dylan", "key1");
if (!flag)
{
    redisManager.HashSet<object>("dylan", "key1", data);
    redisManager.HashSet<object>("dylan", "key2", data);
}
var value = redisManager.HashValues<QueueValue>("dylan");
```

### Sorted类型调用方法
```c# 
var data = new
{
    name = "dylan",
    code = "sharecore",
};
redisManager.SortedSetAdd("dylan", data, 1.0);//添加
redisManager.SortedSetRemove("dylan",data);//删除
redisManager.SortedSetRangeByRank<QueueValue>("dylan");//获取全部
redisManager.SortedSetLength("dylan");//获取数量
```

### Subscribe 与 Publish类型调用方法
订阅
```c# 
redisManager.Subscribe("dylan", (channel, value) =>
{
    Console.WriteLine(channel.ToString() + ":" + value.ToString());
});
```

发布
```c# 
var data = new
{
    name= "dylan",
    code = "sharecore"
};
redisManager.Publish("dylan", data);
```

## DMS.Log4net，DMS.NLogs日志使用
```c# 
public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
    WebHost.CreateDefaultBuilder(args)
    .UseUrls("http://*:9870")
    .UseNLog($"Configs/nlog.config")
    .UseLog4net($"Configs/log4net.config")
    .UseStartup<Startup>();
```

## DMS.Exceptionless 分布式日志
基于Exceptionless框架开发，依赖DMS中基础框架，主要用于分页式日志系统
github源码地址：https://github.com/exceptionless/Exceptionless.Net
参考资料将客户端服务配置起来，目前仅支持ElasticSearch5.X版本

### 1.在appsettings.json文件中添加配置
```c# 
"Exceptionless": {
	"ServerUrl": "http://192.168.0.192:9002",
	"ApiKey": "T1uijEtbtbNcPs0aI1izX6cyscuZbowQKYmEbyUH"
}
```
### 2.消息日志使用示例
```c# 
string tagName = "消息标签";//自定义标签
var data = new ExcDataParam() { Name = "请求参数", Data = new { Id = 001, Name = "张三" } };//自定义单个model
var user = new ExcUserParam() { Id = "No0001", Name = "李廷礼", Email = "litingxian@live.cn", Description = "菁鲤汇高级开发工程师" };//用户信息
var datas = new List<ExcDataParam>()//自定义列表数据
{
	new ExcDataParam(){
		Name="请求参数",
		Data=new { Id = 002, Name = "李四" }
	},
	new ExcDataParam(){
		Name="返回结果",
		Data=new { Id = 003, Name = "王五" }
	}
};

//日志等级：跟踪信息
LessLog.Trace("不带参数");
LessLog.Trace("带标签", tagName);
LessLog.Trace("带用户&标签", user, tagName);
LessLog.Trace("带自定义数据&标签", data, tagName);
LessLog.Trace("带自定义数据&标签", datas, tagName);
LessLog.Trace("带用户&自定义数据&标签", user, data, tagName);
LessLog.Trace("带用户&自定义数据&标签", user, datas, tagName);

//日志等级：调试信息
LessLog.Debug("不带参数");
LessLog.Debug("带标签", tagName);
LessLog.Debug("带用户&标签", user, tagName);
LessLog.Debug("带自定义数据&标签", data, tagName);
LessLog.Debug("带自定义数据&标签", datas, tagName);
LessLog.Debug("带用户&自定义数据&标签", user, data, tagName);
LessLog.Debug("带用户&自定义数据&标签", user, datas, tagName);

//日志等级：普通信息
LessLog.Info("不带参数");
LessLog.Info("带标签", tagName);
LessLog.Info("带用户&标签", user, tagName);
LessLog.Info("带自定义数据&标签", data, tagName);
LessLog.Info("带自定义数据&标签", datas, tagName);
LessLog.Info("带用户&自定义数据&标签", user, data, tagName);
LessLog.Info("带用户&自定义数据&标签", user, datas, tagName);

//日志等级：警告
LessLog.Warn("不带参数");
LessLog.Warn("带标签", tagName);
LessLog.Warn("带用户&标签", user, tagName);
LessLog.Warn("带自定义数据&标签", data, tagName);
LessLog.Warn("带自定义数据&标签", datas, tagName);
LessLog.Warn("带用户&自定义数据&标签", user, data, tagName);
LessLog.Warn("带用户&自定义数据&标签", user, datas, tagName);

//日志等级：致命的
LessLog.Fatal("不带参数");
LessLog.Fatal("带标签", tagName);
LessLog.Fatal("带用户&标签", user, tagName);
LessLog.Fatal("带自定义数据&标签", data, tagName);
LessLog.Fatal("带自定义数据&标签", datas, tagName);
LessLog.Fatal("带用户&自定义数据&标签", user, data, tagName);
LessLog.Fatal("带用户&自定义数据&标签", user, datas, tagName);

//日志等级：异常
LessLog.Error("不带参数");
LessLog.Error("带标签", tagName);
LessLog.Error("带用户&标签", user, tagName);
LessLog.Error("带自定义数据&标签", data, tagName);
LessLog.Error("带自定义数据&标签", datas, tagName);
LessLog.Error("带用户&自定义数据&标签", user, data, tagName);
LessLog.Error("带用户&自定义数据&标签", user, datas, tagName);
```
### 3.异常日志使用示例
```c# 
string tagName = "异常标签";//自定义标签
var data = new ExcDataParam() { Name = "请求参数", Data = new { Id = 001, Name = "张三" } };//自定义单个model
var user = new ExcUserParam() { Id = "No0001", Name = "李廷礼", Email = "litingxian@live.cn", Description = "菁鲤汇高级开发工程师" };//用户信息
var datas = new List<ExcDataParam>()//自定义列表数据
{
	new ExcDataParam(){
		Name="请求参数",
		Data=new { Id = 002, Name = "李四" }
	},
	new ExcDataParam(){
		Name="返回结果",
		Data=new { Id = 003, Name = "王五" }
	}
};
try
{
	LessExceptionLog.Submit("不带参数");
	LessExceptionLog.Submit("带标签", tagName);
	LessExceptionLog.Submit("带用户&标签", user, tagName);
	LessExceptionLog.Submit("带自定义数据&标签", data, tagName);
	LessExceptionLog.Submit("带自定义数据&标签", datas, tagName);
	LessExceptionLog.Submit("带用户&自定义数据&标签", user, data, tagName);
	LessExceptionLog.Submit("带用户&自定义数据&标签", user, datas, tagName);

	//程序逻辑异常测试，用过try catch 捕获
	int number = 0;
	int nubemr1 = 1 / number;
}
catch (Exception ex)
{
	//注意：同一个异常ExceptionsLess只会提交一次，這就说明第二次提交则会无效
	//ex.Submit();//不带任何参数
	//ex.Submit(tagName);//带标签
	//ex.Submit(user, tagName);//带用户&标签
	//ex.Submit(data, tagName);//带自定义数据&标签
	//ex.Submit(datas, tagName);//带自定义数据&标签
	//ex.Submit(user, data, tagName);//带用户&自定义数据&标签
	ex.Submit(user, datas, tagName);//带用户&自定义数据&标签
}
```
### 4.功能特性日志使用示例
```c# 
string tagName = "特性使用标签";//自定义标签
var data = new ExcDataParam() { Name = "请求参数", Data = new { Id = 001, Name = "张三" } };//自定义单个model
var user = new ExcUserParam() { Id = "No0001", Name = "李廷礼", Email = "litingxian@live.cn", Description = "菁鲤汇高级开发工程师" };//用户信息
var datas = new List<ExcDataParam>()//自定义列表数据
{
	new ExcDataParam(){
		Name="请求参数",
		Data=new { Id = 002, Name = "李四" }
	},
	new ExcDataParam(){
		Name="返回结果",
		Data=new { Id = 003, Name = "王五" }
	}
};

LessFeatureLog.Submit("不带参数");
LessFeatureLog.Submit("带标签", tagName);
LessFeatureLog.Submit("带用户&标签", user, tagName);
LessFeatureLog.Submit("带自定义数据&标签", data, tagName);
LessFeatureLog.Submit("带自定义数据&标签", datas, tagName);
LessFeatureLog.Submit("带用户&自定义数据&标签", user, data, tagName);
LessFeatureLog.Submit("带用户&自定义数据&标签", user, datas, tagName);
```
### 5.失效链接日志使用示例
```c# 
string tagName = "失效链接标签";//自定义标签
var data = new ExcDataParam() { Name = "请求参数", Data = new { Id = 001, Name = "张三" } };//自定义单个model
var user = new ExcUserParam() { Id = "No0001", Name = "李廷礼", Email = "litingxian@live.cn", Description = "菁鲤汇高级开发工程师" };//用户信息
var datas = new List<ExcDataParam>()//自定义列表数据
{
	new ExcDataParam(){
		Name="请求参数",
		Data=new { Id = 002, Name = "李四" }
	},
	new ExcDataParam(){
		Name="返回结果",
		Data=new { Id = 003, Name = "王五" }
	}
};
LessLinksLog.Submit("不带参数：http://www.baidu.com");
LessLinksLog.Submit("带标签：http://www.baidu.com", tagName);
LessLinksLog.Submit("带用户&标签：http://www.baidu.com", user, tagName);
LessLinksLog.Submit("带自定义数据&标签：http://www.baidu.com", data, tagName);
LessLinksLog.Submit("带自定义数据&标签：http://www.baidu.com", datas, tagName);
LessLinksLog.Submit("带用户&自定义数据&标签：http://www.baidu.com", user, data, tagName);
LessLinksLog.Submit("带用户&自定义数据&标签：http://www.baidu.com", user, datas, tagName);
```
