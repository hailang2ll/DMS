using DMS.IServices;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Api.Controllers
{
    /// <summary>
    ///  缓存控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class RedisController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IRedisService redisService;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="redisService"></param>

        public RedisController(IRedisService redisService)
        {
            this.redisService = redisService;

        }
        #region redis
        /// <summary>
        /// 我应该在0库
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestRedis0")]
        public async Task TestRedis0(string msg = "我应该在0库")
        {
            await redisService.TestRedis0(msg);
        }
        /// <summary>
        /// 我应该在1库
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestRedis1")]
        public async Task TestRedis1(string msg = "我应该在1库")
        {
            await redisService.TestRedis1(msg);
        }
        #endregion

        #region Subscribe&Publish
        /// <summary>
        /// 订阅
        /// </summary>
        /// <returns></returns>
        [HttpGet("Subscribe")]
        public void Subscribe()
        {
            redisService.Subscribe();
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <returns></returns>
        [HttpGet("Publish")]
        public void Publish()
        {
            redisService.Publish();
        }

        /// <summary>
        /// 订阅服务先注入
        /// 发布
        /// </summary>
        /// <returns></returns>
        [HttpGet("RedisPublish")]
        public void RedisPublish()
        {
            redisService.RedisPublish();
        }
        #endregion
    }
}
