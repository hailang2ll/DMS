using DMS.Authorizations.UserContext;
using DMS.Authorizations.UserContext.Token.FilterAttribute;
using DMS.Common.Helper;
using DMS.Common.JsonHandler;
using DMS.Common.Model.Result;
using DMS.Extensions.Cookies;
using DMS.IServices;
using DMS.IServices.Result;
using DMS.Redis;
using Microsoft.AspNetCore.Mvc;

namespace DMS.Api.Controllers
{
    /// <summary>
    /// 普通TOKEN认证
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    //全局Token认证二选一
    //[TypeFilter(typeof(CheckLoginAttribute))]
    [CheckLoginAuthorizationFilter]
    public class TokenController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IRedisRepository _redisRepository;
        /// <summary>
        /// 
        /// </summary>
        private readonly ICookieHelper _cookieHelper;
        /// <summary>
        /// 
        /// </summary>
        private readonly DMS.Authorizations.UserContext.Token.IUserAuth _userAuth;
        /// <summary>
        /// 
        /// </summary>
        private readonly IProductService _productService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="productService"></param>
        /// <param name="redisRepository"></param>
        /// <param name="cookieHelper"></param>
        /// <param name="userAuth"></param>
        public TokenController(IProductService productService, IRedisRepository redisRepository, ICookieHelper cookieHelper, DMS.Authorizations.UserContext.Token.IUserAuth userAuth)
        {
            _productService = productService;
            _redisRepository = redisRepository;
            _cookieHelper = cookieHelper;
            _userAuth = userAuth;
        }
        /// <summary>
        /// 普通TOKEN认识方式
        /// </summary>
        /// <returns></returns>
        [HttpPost("TokenLogin")]
        [JumpCheckLogin]
        public async Task<ResponseResult> TokenLogin()
        {
            ResponseResult result = new ResponseResult();
            UserTicket tokenModel = new UserTicket
            {
                ID = 120,
                EpCode = "100214545454",
                UID = "435353534",
                ExpDate = DateTime.Now.AddDays(1),
            };
            string sid = DMS.Extensions.UniqueGenerator.UniqueHelper.GetWorkerID().ToString();
            await _redisRepository.SetAsync(sid, tokenModel.SerializeObject(), tokenModel.ExpDate);
            _cookieHelper.SetCookie("AccessToken", sid);
            result.data = sid;
            return result;
        }
        /// <summary>
        /// 普通TOKEN认证模式
        /// </summary>
        /// <returns></returns>
        [HttpGet("CheckTokenAuth")]
        public async Task<ResponseResult> CheckTokenAuth()
        {
            var id = _userAuth.ID;
            var name = _userAuth.Name;
            var epCode = _userAuth.EpCode;
            var uid = _userAuth.UID;
            var token = _userAuth.GetToken();

            var ip = IPHelper.GetCurrentIp();
            return await Task.FromResult(new ResponseResult() { data = new { id, name, epCode, uid, token, ip } });
        }

        /// <summary>
        /// 获取产品信息
        /// 跳过检查身份
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetProduct")]
        [JumpCheckLogin]
        public async Task<ResponseResult<ProductEntityResult>> GetProductAsync(long id)
        {
            var ip = IPHelper.GetCurrentIp();
            return await _productService.GetProductAsync(id);
        }
    }
}
