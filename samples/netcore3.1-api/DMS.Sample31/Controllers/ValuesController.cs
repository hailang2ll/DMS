using DMS.Sample31.Contracts;
using DMSN.Common.BaseResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Sample31.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IProductService productService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productService"></param>
        public ValuesController(IProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        public async Task<ResponseResult<UserEntity>> GetProductAsync(long productID)
        {
            return await productService.GetProductAsync(productID);
        }
    }
}
