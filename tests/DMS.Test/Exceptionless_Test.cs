using DMS.Exceptionless;
using DMS.Exceptionless.Extensions;
using DMS.Exceptionless.Param;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DMS.Test
{
    public class Exceptionless_Test
    {
        /// <summary>
        /// 消息日志测试
        /// </summary>
        [Fact]
        public void MessageTest()
        {
            string tagName = "消息标签";//自定义标签
            var data = new ExcDataParam() { Name = "请求参数", Data = new { Id = 001, Name = "张三" } };//自定义单个model
            var user = new ExcUserParam() { Id = "No0001", Name = "dylan", Email = "dylan@live.cn", Description = "技术" };//用户信息
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
        }

        /// <summary>
        /// 失效链接日志测试
        /// </summary>
        [Fact]
        public void BrokenLinksTest()
        {
            string tagName = "失效链接标签";//自定义标签
            var data = new ExcDataParam() { Name = "请求参数", Data = new { Id = 001, Name = "张三" } };//自定义单个model
            var user = new ExcUserParam() { Id = "No0001", Name = "dylan", Email = "dylan@live.cn", Description = "技术" };//用户信息
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
        }

        /// <summary>
        /// 特性使用日志测试
        /// </summary>
        [Fact]
        public void FeatureUsageTest()
        {
            string tagName = "特性使用标签";//自定义标签
            var data = new ExcDataParam() { Name = "请求参数", Data = new { Id = 001, Name = "张三" } };//自定义单个model
            var user = new ExcUserParam() { Id = "No0001", Name = "dylan", Email = "dylan@live.cn", Description = "技术" };//用户信息
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
        }

        /// <summary>
        /// 异常日志测试
        /// </summary>
        [Fact]
        public void ErrorTest()
        {
            string tagName = "异常标签";//自定义标签
            var data = new ExcDataParam() { Name = "请求参数", Data = new { Id = 001, Name = "张三" } };//自定义单个model
            var user = new ExcUserParam() { Id = "No0001", Name = "dylan", Email = "dylan@live.cn", Description = "技术" };//用户信息
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
        }
    }
}
