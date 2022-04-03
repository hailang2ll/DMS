using DMS.Common.Model.Result;
using DMS.IServices.Param;

namespace DMS.IServices
{
    /// <summary>
    /// 系统日志
    /// </summary>
    public interface ISysLogService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<ResponseResult> Add(AddSysLogParam param);
        Task<ResponseResult> AddTran(AddSysLogParam param);
    }
}
