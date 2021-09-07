using DMS.Auth.Tickets;
using DMS.Redis;
using DMS.Sample31.Contracts;
using DMSN.Common.BaseResult;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Sample31.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IRedisRepository redisRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="redisRepository"></param>
        public ValuesController(IProductService productService, IRedisRepository redisRepository)
        {
            this.productService = productService;
            this.redisRepository = redisRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productID"></param>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        public async Task<ResponseResult<UserEntity>> GetProductAsync(long productID)
        {
            var userEntity = await redisRepository.GetValue<UserTicket>("dylan");
            return await productService.GetProductAsync(productID);
        }

    }
}
