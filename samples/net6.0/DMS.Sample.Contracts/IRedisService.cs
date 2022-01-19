using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Sample.Contracts
{
    /// <summary>
    /// redis 接口
    /// </summary>
    public interface IRedisService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task TestRedis0(string msg = "我应该在0库");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        Task TestRedis1(string msg = "我应该在1库");
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task RedisPublish();
        /// <summary>
        /// 
        /// </summary>
        void Publish();
        /// <summary>
        /// 
        /// </summary>
        void Subscribe();
    }
}
