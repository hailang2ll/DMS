﻿using DMS.Auth.Token.FilterAttribute;
using DMS.Common.Helper;
using DMS.Common.Model.Result;
using DMS.Sample.Contracts;
using DMS.Sample.Contracts.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DMS.Sample.Api.Controllers
{
    /// <summary>
    /// 普通TOKEN认证
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //全局Token认证二选一
    //[TypeFilter(typeof(CheckLoginAttribute))]
    [CheckLoginAuthorizationFilter]
    public class TokenController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly DMS.Auth.Token.IUserAuth userToken;
        /// <summary>
        /// 
        /// </summary>
        private readonly IProductService productService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userToken"></param>
        /// <param name="productService"></param>
        public TokenController(DMS.Auth.Token.IUserAuth userToken, IProductService productService)
        {
            this.userToken = userToken;
            this.productService = productService;
        }

        /// <summary>
        /// 普通TOKEN认证模式
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
            return await Task.FromResult(new ResponseResult() { data = new { id, name, epCode, uid, token, ip } });
        }

        /// <summary>
        /// 获取产品信息
        /// 跳过检查身份
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        [JumpCheckLogin]
        public async Task<ResponseResult<ProductEntityResult>> GetProductAsync(long id)
        {
            var ip = IPHelper.GetCurrentIp();
            return await productService.GetProductAsync(id);
        }
    }
}
