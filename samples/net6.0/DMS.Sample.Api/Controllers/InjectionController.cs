﻿using DMS.Common.Helper;
using DMS.Common.Model.Result;
using DMS.Sample.Contracts;
using DMS.Sample.Contracts.Param;
using DMS.Sample.Contracts.Result;
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
        public IProductService productService1 { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private readonly IProductService _productService;
        /// <summary>
        /// 构造函数注入
        /// </summary>

        public InjectionController(IProductService productService)
        {
            this._productService = productService;
        }

        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        public async Task<ResponseResult<ProductEntityResult>> GetProductAsync(long id)
        {
            var a = productService1.GetProductAsync(id);
            var ip = IPHelper.GetCurrentIp();
            return await _productService.GetProductAsync(id);
        }

        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("SearchProductList")]
        public async Task<ResponseResult<ProductEntityResult>> SearchProductListAsync([FromQuery]SearchProductParam param)
        {
            var ip = IPHelper.GetCurrentIp();
            return await _productService.SearchProductListAsync(param);
        }
    }
}
