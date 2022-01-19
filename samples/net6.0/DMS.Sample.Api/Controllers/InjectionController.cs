﻿using DMS.Common.Helper;
using DMS.Common.Model.Result;
using DMS.Sample.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.Sample.Api.Controllers
{
    /// <summary>
    /// 接口注入
    /// 自定义注入
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InjectionController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IProductService productService;
        /// <summary>
        /// 
        /// </summary>
        private readonly DMS.Auth.v1.IUserAuth userAuth1;
        /// <summary>
        /// 
        /// </summary>
        private readonly DMS.Auth.v2.IUserAuth userAuth2;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="userAuth1"></param>
        /// <param name="userAuth2"></param>

        public InjectionController(IProductService productService,DMS.Auth.v1.IUserAuth userAuth1, DMS.Auth.v2.IUserAuth userAuth2)
        {
            this.productService = productService;
            this.userAuth1 = userAuth1;
            this.userAuth2 = userAuth2;

        }

        #region redis
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestRedis0")]
        public async Task TestRedis0(string msg = "我应该在0库")
        {
            await productService.TestRedis0(msg);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("TestRedis1")]
        public async Task TestRedis1(string msg = "我应该在1库")
        {
            await productService.TestRedis1(msg);
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
            productService.Subscribe();
        }
        /// <summary>
        /// 发布
        /// </summary>
        /// <returns></returns>
        [HttpGet("Publish")]
        public void Publish()
        {
            productService.Publish();
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <returns></returns>
        [HttpGet("RedisPublish")]
        public void RedisPublish()
        {
            productService.RedisPublish();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckAuth")]
        public async Task<ResponseResult> CheckAuth()
        {
            var id = userAuth1.ID;
            var name = userAuth1.Name;
            var token = userAuth1.GetToken();

            var isAuth = userAuth2.IsAuthenticated();
            var id2 = userAuth2.ID2;
            var name2 = userAuth2.Name2;
            var a = DMS.Common.AppConfig.GetValue("AllowedHosts");
            var ip = IPHelper.GetCurrentIp();
            var (loginFlag, result) = await userAuth2.ChenkLoginAsync();
            if (!loginFlag)
            {
                return result;
            }

            return new ResponseResult() { data = new { isAuth, id2, name2 } };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        public async Task<ResponseResult<UserEntity>> GetProductAsync(long productID)
        {
            var ip = IPHelper.GetCurrentIp();
            return await productService.GetProductAsync(productID);
        }
    }
}