using DMS.Common.ControllerExt;
using DMS.Common.Model.Result;
using DMS.IServices;
using DMS.IServices.Param;
using DMS.IServices.Result;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Api.Controllers
{
    /// <summary>
    /// 日志接口
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : BaseApiController
    {
        private readonly ISysLogService _logService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logService"></param>
        public LogController(ISysLogService logService)
        {
            this._logService = logService;
        }
        /// <summary>
        /// 新增工作日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Add")]
        public async Task<ResponseResult> AddAsync(AddSysLogParam param)
        {
            int flag = await _logService.Add0(param);
            return flag > 0 ? Success() : Failed(502, "获取数据失败");
        }

        /// <summary>
        /// 新增工作日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Add0")]
        public async Task<ResponseResult<int>> Add0Async(AddSysLogParam param)
        {
            int flag = await _logService.Add0(param);
            return flag > 0 ? Success(3) : Failed<int>(502, "获取数据失败");
        }

        /// <summary>
        /// 新增工作日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Add1")]
        public async Task<ResponseResult<string>> Add1Async(AddSysLogParam param)
        {
            int flag = await _logService.Add0(param);
            return flag > 0 ? Success("") : Failed<string>(502, "获取数据失败");
        }

        /// <summary>
        /// 新增工作日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Add2")]
        public async Task<ResponseResult<JobLogResult>> Add2Async(AddSysLogParam param)
        {
            JobLogResult result = new JobLogResult() { Id = 12 };
            int a = 9;
            return a > 0 ? Success(result) : Failed<JobLogResult>(502, "获取数据失败");
        }


        /// <summary>
        /// 新增工作日志
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost("Add3")]
        public async Task<ResponseResult<PageModel<JobLogResult>>> Add3Async(AddSysLogParam param)
        {
            PageModel<JobLogResult> result = new PageModel<JobLogResult>();
            int a = 9;
            return a > 0 ? Success(result) : Failed<PageModel<JobLogResult>>(502, "获取数据失败");
        }
    }
}
