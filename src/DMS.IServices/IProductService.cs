using DMS.Common.Model.Result;
using DMS.IServices.Param;
using DMS.IServices.Result;

namespace DMS.IServices
{
    /// <summary>
    /// 产品接口
    /// </summary>
    public interface IProductService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<ProductEntityResult>> GetProductAsync(long id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ResponseResult<PageModel<ProductEntityResult>>> SearchProductListAsync(SearchProductParam param);
    }
}
