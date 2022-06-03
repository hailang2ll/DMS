using DMS.Common.Model.Result;
using DMS.IServices;
using DMS.IServices.Param;
using DMS.IServices.Result;

namespace DMS.Services
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ResponseResult<PageModel<ProductEntityResult>>> SearchProductListAsync(SearchProductParam param)
        {
            ResponseResult<PageModel<ProductEntityResult>> result = new ResponseResult<PageModel<ProductEntityResult>>()
            {
                data = new PageModel<ProductEntityResult>()
            };

            PageModel<ProductEntityResult> pageModel = new PageModel<ProductEntityResult>()
            {
                pageIndex = param.pageIndex,
                pageSize = param.pageSize,
                totalRecord = param.totalCount,
                resultList = new List<ProductEntityResult>(),
            };
            //ProductEntityResult entity = new ProductEntityResult()
            //{
            //    //long类型转为string输出
            //    Id = "1125964271981826048",
            //    ProductName = "aaaa",
            //    ProductPrice = 125.23m,
            //    CreatedTime = DateTime.Now,
            //};

            //pageModel.resultList = GenFu.GenFu.ListOf<ProductEntityResult>(param.pageSize);
            //result.data = pageModel;
            return await Task.FromResult(result);

        }

    }
}
