using DMS.Common.Model.Result;
using DMS.Sample.Contracts.Result;
using System.Threading.Tasks;

namespace DMS.Sample.Contracts
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
    }
}
