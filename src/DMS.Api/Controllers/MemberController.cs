using DMS.Common.Model.Result;
using DMS.IServices;
using DMS.IServices.Param;
using DMS.IServices.Result;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Api.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IYxyMemberService _memberService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberService"></param>
        public MemberController(IYxyMemberService memberService)
        {
            this._memberService = memberService;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="memberID"></param>
        /// <returns></returns>
        [HttpGet("GetMember")]
        public async Task<ResponseResult<YxyMemberResult>> GetMemberAsync(long memberID)
        {
            return await _memberService.GetMemberAsync(memberID);
        }

        /// <summary>
        /// 获取用户集合
        /// </summary>
        /// <param name="memberType"></param>
        /// <returns></returns>
        [HttpGet("GetMemberList")]
        public async Task<ResponseResult<List<YxyMemberResult>>> GetMemberListAsync(long memberType)
        {
            return await _memberService.GetMemberListAsync(memberType);
        }
        /// <summary>
        /// 搜索用户
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>

        [HttpGet("SearchMember")]
        public async Task<ResponseResult<PageModel<YxyMemberResult>>> SearchMemberAsync([FromQuery] SearchYxyMemberParam param)
        {

            return await _memberService.SearchMemberAsync(param);
        }
    }
}
