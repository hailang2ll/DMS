using DMS.Common.Model.Result;
using DMS.IServices.Param;
using DMS.IServices.Result;

namespace DMS.IServices
{
    /// <summary>
    /// 日志接口定义
    /// </summary>
    public interface IYxyMemberService
    {
        Task<ResponseResult> Add(AddMemberParam param);
        Task<ResponseResult> AddTran(AddMemberParam param);
        Task<ResponseResult> GetEntity(long id);
        Task<ResponseResult> GetList(long id);
        Task<ResponseResult> GetList(SearchMemberParam param);
    }
}
