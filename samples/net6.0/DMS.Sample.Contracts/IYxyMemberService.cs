using DMS.Common.Model.Result;
using DMS.Sample.Contracts.Param;
using DMS.Sample.Contracts.Result;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DMS.Sample.Contracts
{
    /// <summary>
    /// 日志接口定义
    /// </summary>
    public interface IYxyMemberService
    {
       
        /// <summary>
        /// 异步查询
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<YxyMemberResult>> GetMemberAsync(long memberID);
        /// <summary>
        /// 异步查询
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<List<YxyMemberResult>>> GetMemberListAsync(long memberType);
        /// <summary>
        /// 异步查询
        /// </summary>
        /// <returns></returns>
        Task<ResponseResult<PageModel<YxyMemberResult>>> SearchMemberAsync(SearchYxyMemberParam param);
    }
}
