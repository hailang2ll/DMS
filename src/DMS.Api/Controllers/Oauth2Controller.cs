﻿using DMS.Authorizations.Model;
using DMS.Common.Helper;
using DMS.Common.Model.Result;
using DMS.IServices;
using DMS.IServices.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Api.Controllers
{
    /// <summary>
    /// jwt认证
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]

    //[Authorize(Roles = "user")]
    //[Authorize(Roles = "admin,user")]
    //[Authorize(Policy = "BaseRole")]
    //[Authorize(Policy = "MoreBaseRole")]
    //[Authorize(Roles = "dylan")]
    //[Authorize(Roles ="system")]
    [Authorize(Policy = Permissions.Name)]
    public class Oauth2Controller : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly DMS.Authorizations.UserContext.Jwt.IUserAuth _userOauth;
        /// <summary>
        /// 
        /// </summary>
        private readonly IProductService _productService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userOauth"></param>
        /// <param name="productService"></param>
        public Oauth2Controller(DMS.Authorizations.UserContext.Jwt.IUserAuth userOauth,  IProductService productService)
        {
            _userOauth = userOauth;
            _productService = productService;
        }

        /// <summary>
        /// 接口不在资源中，认证失败
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckOauth2")]
        public async Task<ResponseResult> CheckOauth2()
        {
            var isAuth = _userOauth.IsAuthenticated();
            var id2 = _userOauth.Uid;
            var token = _userOauth.GetToken();
            var a = DMS.Common.AppConfig.GetValue("AllowedHosts");

            return await Task.FromResult(new ResponseResult() { data = new { isAuth, id2 } });
        }
        /// <summary>
        /// 需要认证，接口必须在资源中
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProduct1")]
        public async Task<ResponseResult<ProductEntityResult>> GetProduct1Async(long id)
        {
            var isAuth = _userOauth.IsAuthenticated();
            var id2 = _userOauth.Uid;
            var token = _userOauth.GetToken();
            var ep = _userOauth.EpCode;
            var ip = IPHelper.GetCurrentIp();
            return await _productService.GetProductAsync(id);
        }

        /// <summary>
        /// 获取产品信息
        /// 跳过检查身份
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        [AllowAnonymous]
        public async Task<ResponseResult<ProductEntityResult>> GetProductAsync(long id)
        {
            var ip = IPHelper.GetCurrentIp();
            return await _productService.GetProductAsync(id);
        }
    }
}
