﻿using DMS.Common.Helper;
using DMS.Common.Model.Result;
using DMS.Sample.Contracts;
using DMS.Sample.Contracts.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.Sample.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "user")]
    //[Authorize(Roles = "admin,user")]
    //[Authorize(Policy = "BaseRole")]
    //[Authorize(Policy = "MoreBaseRole")]
    //[Authorize(Roles = "dylan")]
    //[Authorize(Roles ="system")]
    //[Authorize(Policy = "customizePermisson")]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly DMS.Auth.Token.IUserAuth userToken;
        /// <summary>
        /// 
        /// </summary>
        private readonly DMS.Auth.Oauth2.IUserAuth userOauth;
        /// <summary>
        /// 
        /// </summary>
        private readonly IProductService productService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="userOauth"></param>
        /// <param name="productService"></param>
        public AuthController(DMS.Auth.Token.IUserAuth userToken, DMS.Auth.Oauth2.IUserAuth userOauth, IProductService productService)
        {
            this.userToken = userToken;
            this.userOauth = userOauth;
            this.productService = productService;
        }

        /// <summary>
        ///  普通TOKEN认证模式
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckTokenAuth")]
        public async Task<ResponseResult> CheckTokenAuth()
        {
            var id = userToken.ID;
            var name = userToken.Name;
            var epCode = userToken.EpCode;
            var uid = userToken.UID;
            var token = userToken.GetToken();

            var ip = IPHelper.GetCurrentIp();
            if (id <= 0)
            {
                return new ResponseResult() { errno = 30, errmsg = "未登录" };
            }

            return await Task.FromResult(new ResponseResult() { data = new { id, name, epCode, uid, token, ip } });
        }

        /// <summary>
        /// Oauth2认证模式
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckOauth2")]
        //[TypeFilter(typeof(CheckLoginAttribute))]
        public async Task<ResponseResult> CheckOauth2()
        {
            var isAuth = userOauth.IsAuthenticated();
            var id2 = userOauth.ID;
            var name2 = userOauth.Name;
            var token = userOauth.GetToken();
            var a = DMS.Common.AppConfig.GetValue("AllowedHosts");
            var ip = IPHelper.GetCurrentIp();
            //var (loginFlag, result) = await userOauth.ChenkLoginAsync();
            //if (!loginFlag)
            //{
            //    return result;
            //}

            return await Task.FromResult(new ResponseResult() { data = new { isAuth, id2, name2 } });
        }

        /// <summary>
        /// 获取产品信息
        /// 验证身份令牌
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        public async Task<ResponseResult<ProductEntityResult>> GetProductAsync(long id)
        {
            var ip = IPHelper.GetCurrentIp();
            return await productService.GetProductAsync(id);
        }
    }
}