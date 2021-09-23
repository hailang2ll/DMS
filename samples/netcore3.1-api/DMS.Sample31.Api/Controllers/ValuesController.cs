using DMS.Auth;
using DMS.Sample31.Contracts;
using DMSN.Common.BaseResult;
using DMSN.Common.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Sample31.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "admin")]
    //[Authorize(Roles = "user")]
    //[Authorize(Roles = "admin,user")]
    //[Authorize(Policy = "BaseRole")]
    //[Authorize(Policy = "MoreBaseRole")]
    //[Authorize(Policy = "customizePermisson")]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IUserAuth userAuth;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="userAuth"></param>

        public ValuesController(IProductService productService, IUserAuth userAuth)
        {
            this.productService = productService;
            this.userAuth = userAuth;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "0Catcher Wong", "James Li" };
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
        //[TypeFilter(typeof(CheckLoginAttribute))]
        public async Task<ResponseResult> CheckAuth()
        {
            var token = userAuth.GetToken();
            var isAuth = userAuth.IsAuthenticated();
            var id2 = userAuth.ID2;
            var name2 = userAuth.Name2;
            var a = DMSN.Common.CoreExtensions.AppConfig.GetVaule("AllowedHosts");
            var ip = IPHelper.GetCurrentIp();
            var (loginFlag, result) = await userAuth.ChenkLoginAsync();
            if (!loginFlag)
            {
                return result;
            }
            var id = userAuth.ID;
            var name = userAuth.Name;
            return new ResponseResult() { data = new { isAuth, id2, name2 } };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        [AllowAnonymous]
        public async Task<ResponseResult<UserEntity>> GetProductAsync(long productID)
        {
            var ip = IPHelper.GetCurrentIp();
            return await productService.GetProductAsync(productID);
        }



    }
}
