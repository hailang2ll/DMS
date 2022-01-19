using DMS.Auth;
using DMS.Common.Model.Result;
using DMS.Redis;
using DMS.Sample.Contracts;
using DMS.Sample.Service.RedisEvBus;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DMS.Common.JsonHandler;
using DMS.Sample.Contracts.Result;

namespace DMS.Sample.Service
{
    /// <summary>
    /// 产品服务
    /// </summary>
    public class ProductService : IProductService
    {
        /// <summary>
        /// 获取产品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseResult<ProductEntityResult>> GetProductAsync(long id)
        {
            ResponseResult<ProductEntityResult> result = new ResponseResult<ProductEntityResult>();
            ProductEntityResult entity = new ProductEntityResult()
            {
                //long类型转为string输出
                Id = "1125964271981826048",
                ProductName = "aaaa",
                ProductPrice = 125.23m,
                CreatedTime = DateTime.Now,
            };
            result.data = entity;
            return await Task.FromResult(result);

        }
       

    }
}
