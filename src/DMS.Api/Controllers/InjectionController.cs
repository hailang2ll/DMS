using DMS.Common.Helper;
using DMS.Common.Model.Result;
using DMS.IServices;
using DMS.IServices.Param;
using DMS.IServices.Result;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace DMS.Api.Controllers
{
    /// <summary>
    /// autofac
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class InjectionController : ControllerBase
    {
        /// <summary>
        /// 属性注入
        /// </summary>
        public IProductService _productService1 { get; set; }
        /// <summary>
        /// 构造函数注入
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
        /// 我是构造函数注入
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        public async Task<ResponseResult<ProductEntityResult>> GetProductAsync([Required] long id)
        {
            var ip = IPHelper.GetCurrentIp();
            bool mockEnable = DMS.Common.AppConfig.GetValue<bool>("MockEnable");
            if (mockEnable)
            {
                GenFu.GenFu.Configure<ProductEntityResult>().Fill(b => b.ProductName, () => { return "全局替换"; });
                DateTime dateTime = DateTime.Now;
                var result = new ResponseResult<ProductEntityResult>()
                {
                    data = GenFu.GenFu.New(new ProductEntityResult { CreatedTime = dateTime }),
                };
                return result;
            }
            return await _productService.GetProductAsync(id);
        }

        /// <summary>
        /// 我是属性注入
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProduct1")]
        public async Task<ResponseResult<ProductEntityResult>> GetProduct1Async(long id)
        {
            return await _productService1.GetProductAsync(id);
        }

        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet("SearchProductList")]
        public async Task<ResponseResult<PageModel<ProductEntityResult>>> SearchProductListAsync([FromQuery] SearchProductParam param)
        {
            bool mockEnable = DMS.Common.AppConfig.GetValue<bool>("MockEnable");
            if (mockEnable)
            {
                GenFu.GenFu.Configure<ProductEntityResult>().Fill(b => b.ProductName, () => { return "全局替换"; });
                DateTime dateTime = DateTime.Now;
                var result = new ResponseResult<PageModel<ProductEntityResult>>()
                {
                    data = new PageModel<ProductEntityResult>()
                    {
                        pageIndex = param.pageIndex,
                        pageSize = param.pageSize,
                        totalRecord = param.totalCount,
                        resultList = GenFu.GenFu.ListOf<ProductEntityResult>(param.pageSize),
                    }
                };
                return result;
            }
            var ip = IPHelper.GetCurrentIp();
            return await _productService.SearchProductListAsync(param);
        }

        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("SearchProductList1")]
        public async Task<ResponseResult<PageModel<ProductEntityResult>>> SearchProductList1Async([FromBody] SearchProductParam param)
        {
            var ip = IPHelper.GetCurrentIp();
            return await _productService.SearchProductListAsync(param);
        }
    }
}
