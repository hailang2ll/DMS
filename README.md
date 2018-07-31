## DMS

### DMS将会是一个集中式的中间件框架，每一个小型中间件将会是完全独立的，如：gRPC，Thrift，netty，Wcf，Exceptionless，Ocelot，RabbitMQ，Redis，IdentityServer，Consul，Zookeeper等，Demo中会有每一个中间件的实例方便开发者了解 

## Exceptionless 分布式日志说明
github源码地址：https://github.com/exceptionless/Exceptionless.Net
参考资料将客户端服务配置起来，目前仅支持ElasticSearch5.X版本

### DMS.Exceptionless
```c# 
LessLog.Info("这是一条提示信息");
LessLog.Error("这是一条错误的信息");
LessLog.Fatal("这是一条致命的信息");
```